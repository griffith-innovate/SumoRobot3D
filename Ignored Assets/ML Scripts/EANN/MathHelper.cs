#region Includes
using System;
#endregion


/*
================================================================================
Static class for different Math operations and constants.


Original code by Samuel Arzt (March 2017) at:
https://github.com/ArztSamuel/Applying_EANNs
================================================================================
 */

public static class MathHelper {
    #region Methods
    #region Activation Functions

    // The standard sigmoid function.
    public static double SigmoidFunction(double xValue) {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return 0.0;
        else return 1.0 / (1.0 + Math.Exp(-xValue));
    }


    // The standard TanH function.
    public static double TanHFunction(double xValue) {
        if (xValue > 10) return 1.0;
        else if (xValue < -10) return -1.0;
        else return Math.Tanh(xValue);
    }


    // The SoftSign function as proposed by Xavier Glorot and Yoshua Bengio (2010): 
    // "Understanding the difficulty of training deep feedforward neural networks".
    public static double SoftSignFunction(double xValue) {
        return xValue / (1 + Math.Abs(xValue));
    }
    #endregion
    #endregion
}

