import tensorflow as tf
import os
import datetime

def createModel() :

  initializer = tf.keras.initializers.TruncatedNormal(mean=0., stddev=0.1)

  model = tf.keras.models.Sequential([
    tf.keras.layers.Flatten(input_shape=(28, 28)),
    tf.keras.layers.Reshape((28, 28, 1)),  
    tf.keras.layers.Conv2D(filters=32, kernel_size=5, strides=(1,1), activation='relu', use_bias=True, kernel_initializer=initializer,padding='same'),
    tf.keras.layers.MaxPooling2D(pool_size=(3,3), strides=(2,2), padding='same'),
    tf.keras.layers.Conv2D(filters=64, kernel_size=5, strides=(1,1), activation='relu', use_bias=True, kernel_initializer=initializer,padding='same'),  
    tf.keras.layers.MaxPooling2D(pool_size=(3,3), strides=(2,2), padding='same'), 
    tf.keras.layers.Reshape((-1, 7*7*64)),
    tf.keras.layers.Dense(1024, activation='relu', use_bias=True, kernel_initializer=initializer),
    tf.keras.layers.Dropout(rate=0.5),
    tf.keras.layers.Dense(10, activation='softmax')
  ])

  opt = tf.keras.optimizers.Adam(learning_rate=0.0001)

  model.compile(optimizer=opt,
                loss='sparse_categorical_crossentropy',
                metrics=['accuracy'])

  model.summary()
  return model