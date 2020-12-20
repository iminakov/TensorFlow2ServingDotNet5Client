using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tensorflow.Serving;
using TensorFlowServingClient.Utils;
using TensorFlowServingCSharpClientNet5.Models;
using WebTensorFlowServingClient.Models;

namespace TensorFlowServingCSharpClientNet5.Controllers
{
    [Route("api/[controller]")]
    public class MnistDeepController : Controller
    {
        private IConfiguration _configuration;
        private HttpClient _httpClient;

        public MnistDeepController(IConfiguration Configuration, HttpClient httpClient)
        {
            _configuration = Configuration;
            _httpClient = httpClient;
        }

        [HttpPost("[action]")]
        public PredictionResult PredictNumberByGrpc([FromBody] PredictionRequest model)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var imageData = CreateImageDataFromModel(model.ImageData);

                //Init predict request
                var request = new PredictRequest()
                {
                    ModelSpec = new ModelSpec() { Name = "mnist_v1", SignatureName = "serving_default" }
                };
                request.Inputs.Add("flatten_input", TensorBuilder.CreateTensorFromImage(imageData, 255.0f));

                //Create grpc request
                var channel = new Channel(_configuration.GetSection("TfServer")["GrpcServerUrl"], ChannelCredentials.Insecure);
                var client = new PredictionService.PredictionServiceClient(channel);
                var predictResponse = client.Predict(request);

                //Process response
                var maxValue = predictResponse.Outputs["dense_1"].FloatVal.Max();
                var predictedValue = predictResponse.Outputs["dense_1"].FloatVal.IndexOf(maxValue);

                return new PredictionResult()
                {
                    Success = true,
                    Results = predictResponse.Outputs["dense_1"].FloatVal.Select(x => x).ToList(),
                    PredictedValue = predictedValue,
                    DebugText = $"Total time: {stopWatch.ElapsedMilliseconds} ms"
                };

            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPost("[action]")]
        public async Task<PredictionResult> PredictNumberByRest([FromBody] PredictionRequest model)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var imageData = CreateImageDataFromModel(model.ImageData);

                var request = new PredictionRestRequest();
                request.SignatureName = "serving_default";
                request.Instances = TensorBuilder.CreateTensorFromImage(imageData, 255.0f).FloatVal.ToList();

                var result = await _httpClient.PostAsync(_configuration.GetSection("TfServer")["RestServerUrl"],
                    new StringContent(JsonConvert.SerializeObject(request, Formatting.Indented)));

                //Process response
                var content = await result.Content.ReadAsStringAsync();
                var predictionRestResponse = JsonConvert.DeserializeObject<PredictionRestResponse>(content);
                var maxValue = predictionRestResponse.Predictions[0][0].Max();
                var predictedValue = Array.IndexOf(predictionRestResponse.Predictions[0][0], maxValue);

                return new PredictionResult()
                {
                    Success = true,
                    Results = predictionRestResponse.Predictions[0][0].Select(x => x).ToList(),
                    PredictedValue = predictedValue,
                    DebugText = $"Total time: {stopWatch.ElapsedMilliseconds} ms"
                };
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        private int[][] CreateImageDataFromModel(string modelImageData)
        {
            //Load Bitmap from input base64
            Bitmap convertedImage = null;

            using (var str = new MemoryStream(Convert.FromBase64String(modelImageData)))
            {
                str.Position = 0;
                using (var bmp = Image.FromStream(str))
                {
                    //Resize image and convert to rgb24
                    convertedImage = ImageUtils.ResizeImage(bmp, 28, 28, 280, 280);
                }
            }

            //Convert image to 28x28 8bit per pixel image data array
            var imageData = ImageUtils.ConvertImageStreamToDimArraysGrayScale(convertedImage);

            return imageData;
        }

        private PredictionResult ErrorResult(Exception ex) => new PredictionResult()
        {
            Success = false,
            ErrorMessage = ex.ToString()
        };
    }
}
