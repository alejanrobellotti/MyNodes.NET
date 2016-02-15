﻿//planer-pro copyright 2015 GPL - license.

using System;
using System.IO;

namespace MyNetSensors.Nodes
{
    public class SystemFileNode : Node
    {
        public SystemFileNode() : base("System", "File")
        {
            AddInput("File Name", DataType.Text);
            AddInput("Text", DataType.Text,true);
            AddInput("Read", DataType.Logical, true);
            AddInput("Write", DataType.Logical, true);
            AddInput("Clear", DataType.Logical, true);

            AddOutput("Text");

            options.LogOutputChanges = false;
            options.ProtectedAccess = true;
        }

        public override void OnInputChange(Input input)
        {
            //delete
            if (input == Inputs[4] && input.Value == "1")
            {
                var fileName = Inputs[0].Value;
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception)
                {
                    LogError($"Failed to delete file [{fileName}]");
                }
                Outputs[0].Value = null;
            }

            //write
            if (input == Inputs[3] && input.Value == "1")
            {
                var fileName = Inputs[0].Value;
                try
                {
                    var text = Inputs[1].Value;
                    File.AppendAllText(fileName, text);
                }
                catch (Exception)
                {
                    LogError($"Failed to write file [{fileName}]");
                }
            }

            //read
            if (input == Inputs[2] && input.Value == "1")
            {
                var fileName = Inputs[0].Value;
                try
                {
                    var text = File.ReadAllText(fileName);
                    Outputs[0].Value = text;
                }
                catch (Exception)
                {
                    LogError($"Failed to read file [{fileName}]");
                    Outputs[0].Value = null;
                }
            }
        }
    }
}