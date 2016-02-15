﻿/*  MyNetSensors 
    Copyright (C) 2015 Derwish <derwish.pro@gmail.com>
    License: http://www.gnu.org/licenses/gpl-3.0.txt  
*/

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetSensors.Nodes
{
    public class SystemBeepAdvancedNode : Node
    {
        public SystemBeepAdvancedNode() : base("System", "Beep Advanced")
        {
            AddInput("Start", DataType.Logical);
            AddInput("Frequency", DataType.Number);
            AddInput("Duration", DataType.Number);
        }

        public override void OnInputChange(Input input)
        {
            if (Inputs.Any(i => i.Value == null))
            {
                return;
            }

            if (Inputs[0].Value == "1")
            {
                try
                {
                    var f = int.Parse(Inputs[1].Value);
                    var d = int.Parse(Inputs[2].Value);

                    f = f < 37 ? 37 : f > 32767 ? 32767 : f;

                    Beep(f, d);
                    LogInfo($"Beep");
                }
                catch
                {
                    LogError($"Incorrect value in input");
                }
            }
        }

        public async void Beep(int freq, int dur)
        {
            await Task.Run(() => Console.Beep(freq, dur));
        }
    }
}