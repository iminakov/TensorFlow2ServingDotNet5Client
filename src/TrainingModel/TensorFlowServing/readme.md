docker run -t --rm -p 8501:8501 -v "D:/Projects/Reseach/serving/tensorflow_serving/servables/tensorflow/testdata/saved_model_half_plus_two_cpu:/models/half_plus_two" -e MODEL_NAME=half_plus_two tensorflow/serving &

curl -g -d @data.json -X POST http://localhost:8501/v1/models/half_plus_two:predict