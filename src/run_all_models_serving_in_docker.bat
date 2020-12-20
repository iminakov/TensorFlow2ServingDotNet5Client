docker run --rm -p 8500:8500 -p 8501:8501 ^
	--mount type=bind,source="%cd%/TrainingModel/ExportedCatsAndDogsModel",target="/models/ExportedCatsAndDogsModel" ^
	--mount type=bind,source="%cd%/TrainingModel/ExportedModel",target="/models/ExportedModel" ^
	--mount type=bind,source="%cd%/TrainingModel/models.config",target="/models/models.config" ^
    -t tensorflow/serving ^
	--model_config_file="/models/models.config"