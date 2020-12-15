# Tensorflow 2.0 serving .Net Core 5 React/Redux client (GRPC/Rest)
Tensor Flow 2.0 Serving C# client example with gRPC and Rest. There are MNIST CNN prediction model example and ASP.NET Core 5.0 client application with ReactJS/Redux/Typescript front end.

## Repository contains the following content:
- [learning](src/TrainingModel) - python scripts with MNIST CNN model preparing using Keras and Tensorflow 2.0.
- [ClientBaseLib](src/DotnetClient/TensorFlowServingClient) - base library with TF Serving gRPC generated classes and utils classes to create Tensors on C# .NET Core 5 . (Code compatible with TF 1.0)
- [ASP.NET Core 5.0/ ReactJS/Redux Client](src/DotnetClient/TensorFlowServingCSharpClientNet5) - SPA application gRPC/Rest clients for MNIST prediction TensorFlow 2.0 Serving 

## Environment requirements for running example:
- Docker 
- Windows 10
- MSVS 2019 
- VS Code

## How to run example
- Run command promt in root directory in repository
```sh
cd src
```

- Init TensorFlowServing docker image with code (Required only first time)
```sh
init_tensorflow_serving_in_docker.bat
```

- Run trained model in TensorFlowServing docker container
```sh
start run_serving_in_docker.bat
```

- Build and Run .Net Client Application
```sh
start run_client.bat
```

- Navigate to [http://localhost:5000/](http://localhost:5000/) and test prediction app

![.NET Core 5.0 TensorFlow 2.0 Client](https://user-images.githubusercontent.com/2061634/102206464-ba58ac00-3edd-11eb-8c57-6095091dec12.png)