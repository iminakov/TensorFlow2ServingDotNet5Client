import tensorflow as tf
import os
import datetime
from createModel import *

mnist = tf.keras.datasets.mnist

(x_train, y_train), (x_test, y_test) = mnist.load_data()
x_train, x_test = x_train / 255.0, x_test / 255.0

checkpoint_path = "results/training_20201209-232437/cp-{epoch:04d}.ckpt"
checkpoint_dir = os.path.dirname(checkpoint_path)

model2 = createModel()

latest = tf.train.latest_checkpoint(checkpoint_dir)
model2.load_weights(latest)

# Заново оценим модель
loss, acc = model2.evaluate(x_test,  y_test, verbose=2)
print("Restored model, accuracy: {:5.2f}%".format(100*acc))

export_path = os.path.dirname("exports/1/")
print('export_path = {}\n'.format(export_path))

tf.keras.models.save_model(
    model2,
    export_path,
    overwrite=True,
    include_optimizer=True,
    save_format=None,
    signatures=None,
    options=None
)
