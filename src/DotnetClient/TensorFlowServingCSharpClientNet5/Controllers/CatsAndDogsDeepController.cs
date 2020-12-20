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
    public class CatsAndDogsDeepController : Controller
    {
        private IConfiguration _configuration;
        private HttpClient _httpClient;

        public CatsAndDogsDeepController(IConfiguration Configuration, HttpClient httpClient)
        {
            _configuration = Configuration;
            _httpClient = httpClient;
        }

        [HttpPost("[action]")]
        public PredictionResult PredictByGrpc([FromBody] PredictionRequest model)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var imageData = CreateImageDataFromModel(model.ImageData);

                //Init predict request
                var request = new PredictRequest()
                {
                    ModelSpec = new ModelSpec() { Name = "cats_and_dogs", SignatureName = "serving_default" }
                };
                request.Inputs.Add("input_2", TensorBuilder.CreateTensorFromImage(imageData, 255.0f));

                //Create grpc request
                var channel = new Channel(_configuration.GetSection("TfServerCatsAndDogs")["GrpcServerUrl"], ChannelCredentials.Insecure);
                var client = new PredictionService.PredictionServiceClient(channel);
                var predictResponse = client.Predict(request);

                //Process response
                var maxValue = predictResponse.Outputs["dense"].FloatVal.Max();
                var predictedValue = predictResponse.Outputs["dense"].FloatVal.IndexOf(maxValue);

                return new PredictionResult()
                {
                    Success = true,
                    Results = predictResponse.Outputs["dense"].FloatVal.Select(x => x).ToList(),
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
        public async Task<PredictionResult> PredictByRest([FromBody] PredictionRequest model)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var imageData = CreateImageDataFromModel(model.ImageData);

                var tensors = TensorBuilder.CreateTensorFromImage(imageData, 255.0f).FloatVal.ToList();

                var request = new PredictionRestColorImageRequest();
                request.SignatureName = "serving_default";
                request.Instances = new float[1][][][];
                request.Instances[0] = new float[160][][];

                for (int i = 0; i < 160; i++)
                {
                    request.Instances[0][i] = new float[160][];
                    for (int j = 0; j < 160; j++)
                    {
                        request.Instances[0][i][j] = new float[3];
                        request.Instances[0][i][j][0] = tensors[(i*160 + j) * 3];
                        request.Instances[0][i][j][1] = tensors[(i*160 + j) * 3 + 1];
                        request.Instances[0][i][j][2] = tensors[(i*160 + j) * 3 + 2];
                    }
                }

                var result = await _httpClient.PostAsync(_configuration.GetSection("TfServerCatsAndDogs")["RestServerUrl"],
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

        private (int, int, int)[][] CreateImageDataFromModel(string modelImageData)
        {
            Bitmap convertedImage = null;

            using (var str = new MemoryStream(Convert.FromBase64String(modelImageData)))
            {
                str.Position = 0;
                using (var bmp = Image.FromStream(str))
                {
                    convertedImage = ImageUtils.ResizeImage(bmp, 160, 160, bmp.Width, bmp.Height);
                }
            }

            var imageData = ImageUtils.ConvertImageStreamToDimArraysColor(convertedImage);

            return imageData;
        }

        private PredictionResult ErrorResult(Exception ex) => new PredictionResult()
        {
            Success = false,
            ErrorMessage = ex.ToString()
        };
    }
}
