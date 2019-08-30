# SumoRobot3D

SumoRobot3D

This Project uses the ML-Agents Plugin Library from Unity to connect and train Neural Networks for use in the simulation. To install this plugin, follow the instructions found at: [ML-Agents Docs](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Installation.md).
The TL:DR of these instructions are:

### Requirements

-   Python 3.6
-   TensorFlow

### Installation

1. Download the ML-Agents Repo and add it to the assets folder of your unity project.
2. In your preferred Python environment, install the python api package by running:

```python
pip install mlagents
```

## Training a neural network using ML-Agents:

Follow the directions [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Create-New.md) from instruction `Implement an Academy` to setup your project to be ready to train and run the nerual network. This project should be setup to go, however it is a good idead to check the instructions to make sure everything is configured correctly.

TLDR: To train using the simulation:

1. Select Academy in the Hierarchy window, add RollerBallBrain and RollerBallPlayer to the Brains section of the Academy Script in the inspect window.
2. Make sure `Control` is ticked for the RollerBallBrain option
3. Open a terminal window and navigate to the folder containing the project
4. Run the following command: `mlagents-learn trainer_config.yaml --train --run-id="inital_run"
5. Once the terminal displays the message `Start Training by pressing the play button in the unity editor` do so.

The training will now take place, by default it is performed at 100x speed so the video framerate in the unity editor will be choppy at best. to slow it down and see what is happening, Change the Time Scale option in the Training Configuration of the Academy Object.

For each training epoch, the resulting scores of the NN so far will be displayed. Once you are satisified with the results, Press the play button again or Ctrl-C in the Terminal to stop the training and a model will be prepared and finished for you in the Models Folder.

## Inference/Testing a Model.

1. To test a model go the academy object and untick the Control box for the learning brain.
2. Go to the brain object and under option Model, select the Neural Network created by the training process.
3. Start the application using the play button. The Neural Network will now control the player object in the manner it has been trained.
