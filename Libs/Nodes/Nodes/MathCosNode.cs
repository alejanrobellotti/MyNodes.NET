﻿/*  MyNetSensors 
    Copyright (C) 2015 Derwish <derwish.pro@gmail.com>
    License: http://www.gnu.org/licenses/gpl-3.0.txt  
*/

using System;

namespace MyNetSensors.Nodes
{
    public class MathCosNode : Node
    {
        public MathCosNode() : base("Math", "Cos")
        {
            AddInput(DataType.Number);
            AddOutput(DataType.Number);

            options.ResetOutputsIfAnyInputIsNull = true;
        }

        public override void OnInputChange(Input input)
        {
            var a = double.Parse(Inputs[0].Value);
            var b = Math.Cos(a);

            Outputs[0].Value = b.ToString();
        }
    }
}