import tensorflow as tf
import os
import datetime

mnist = tf.keras.datasets.mnist

(x_train, y_train), (x_test, y_test) = mnist.load_data()
x_train, x_test = x_train / 255.0, x_test / 255.0

import json
data = json.dumps({"signature_name": "serving_default", "instances": x_test[0:1].tolist()})

print('Data: {} ... {}'.format(data[:50], data[len(data)-252:]))