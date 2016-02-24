﻿/*  MyNetSensors 
    Copyright (C) 2016 Derwish <derwish.pro@gmail.com>
    License: http://www.gnu.org/licenses/gpl-3.0.txt  
*/

namespace MyNetSensors.Nodes
{
    public class FiltersOnlySpecifiedNode : Node
    {
        public FiltersOnlySpecifiedNode() : base("Filters", "Only Specified")
        {
            AddInput("Value");
            AddInput("Sample");
            AddOutput();
        }


        public override void OnInputChange(Input input)
        {
            if (input == Inputs[0] && input.Value == Inputs[1].Value)
                Outputs[0].Value = input.Value;
        }

        public override string GetNodeDescription()
        {
            return "This node filters the input values. " +
                   "It transmits the value from input named \"Value\" " +
                   "only if it is equal to \"Sample\".";
        }
    }
}