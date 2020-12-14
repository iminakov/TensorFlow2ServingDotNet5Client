import tensorflow as tf
import os
import datetime
from createModel import *

mnist = tf.keras.datasets.mnist

(x_train, y_train), (x_test, y_test) = mnist.load_data()
x_train, x_test = x_train / 255.0, x_test / 255.0

checkpoint_path = "results/training_"+datetime.datetime.now().strftime("%Y%m%d-%H%M%S")+"/cp-{epoch:04d}.ckpt"
checkpoint_dir = os.path.dirname(checkpoint_path)

# Создаем коллбек сохраняющий веса модели
cp_callback = tf.keras.callbacks.ModelCheckpoint(filepath=checkpoint_path,
                                                 save_weights_only=True,
                                                 verbose=1)

log_dir = "logs/fit/" + datetime.datetime.now().strftime("%Y%m%d-%H%M%S")
tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir=log_dir, histogram_freq=1)

current_accuracy = 0

model = createModel()

while True :
    history = model.fit(x_train, y_train, callbacks=[tensorboard_callback, cp_callback])
    acc_val = history.history['accuracy'][-1]
    if acc_val > current_accuracy :
      current_accuracy = acc_val
    else:
      break

model.evaluate(x_test,  y_test, verbose=2)