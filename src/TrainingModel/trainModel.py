import tensorflow as tf
import os
import datetime
from createModel import *

# Load test data
mnist = tf.keras.datasets.mnist
(x_train, y_train), (x_test, y_test) = mnist.load_data()
x_train, x_test = x_train / 255.0, x_test / 255.0

# Create callback for saving train process chekpoints
checkpoint_path = "TrainCheckpoints/training_"+datetime.datetime.now().strftime("%Y%m%d-%H%M%S")+"/cp-{epoch:04d}.ckpt"
checkpoint_dir = os.path.dirname(checkpoint_path)
cp_callback = tf.keras.callbacks.ModelCheckpoint(filepath=checkpoint_path,
                                                 save_weights_only=True,
                                                 verbose=1)

# Create callback for tensor board staff
log_dir = "logs/fit/" + datetime.datetime.now().strftime("%Y%m%d-%H%M%S")
tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir=log_dir, histogram_freq=1)

# Create callback for tensor board staff
model = createModel()

# Train model until accurancy will to be better than later
current_accuracy = 0
while True :
    history = model.fit(x_train, y_train, callbacks=[tensorboard_callback, cp_callback])
    acc_val = history.history['accuracy'][-1]
    if acc_val > current_accuracy :
      current_accuracy = acc_val
    else:
      break

# Test trained model with test data
model.evaluate(x_test,  y_test, verbose=2)

# Export trained model to ExportedModel/1 directory
export_path = os.path.dirname("ExportedModel/1/")
print('exported_model_path = {}\n'.format(export_path))

tf.keras.models.save_model(
    model,
    export_path,
    overwrite=True,
    include_optimizer=True,
    save_format=None,
    signatures=None,
    options=None
)