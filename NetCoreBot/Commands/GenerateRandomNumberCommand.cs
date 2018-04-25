﻿using NetCoreBot.Commands.Abstract;
using NetCoreBot.Resources;
using NetCoreBot.Repository.Concrete;
using NetCoreBot.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBot.Commands.Concrete;
using NetCoreBot.Common.Abstract;
using NetCoreBot.Common.Concrete;
using System.Threading.Tasks;

namespace NetCoreBot.Commands
{
    class GenerateRandomNumberCommand : ICommand
    {
        ISettings _settings;

        private List<string> _parameters;
        private object _messageDetails;

        Random random = new Random();

        public GenerateRandomNumberCommand(List<string> parameters, object messageDetails)
        {
            _parameters = parameters;
            _messageDetails = messageDetails;
            _settings = Settings.Instance;
        }

        public async Task Execute()
        {
            IMessageWriter _writer = new MessageWriter(_messageDetails);
            int? rng = null;
            string error = "Invalid use of command!";
            if (AreValid(_parameters))
            {
                List<int> _intParameters = ConvertStringToInt(_parameters);
                if (_intParameters.Count == 0)
                    rng = Generate();
                if (_intParameters.Count == 1)
                    rng = Generate(0, _intParameters[0]);
                if (_intParameters.Count == 2)
                    rng = Generate(_intParameters[0], _intParameters[1]);
                if (rng == null)
                    await _writer.ReturnStatus(error);
                else
                    await _writer.ReturnStatus(rng.ToString());
            }
            else
            {
                await _writer.ReturnStatus(error);
            }
            await Task.CompletedTask;
        }

        public string Help()
        {
            string help = "Usage:\n"
                + _settings.GetValue(SettingKeys.CommandPrefix) + " random - Generates integer number from 0 to 100.\n"
                + _settings.GetValue(SettingKeys.CommandPrefix) + " random [max] - Generates integer number from 0 to max.\n"
                + _settings.GetValue(SettingKeys.CommandPrefix) + " random [min] [max] - Generates integer number from min to max.\n";
            return help;
        }

        private bool AreValid(List<string> parameters)
        {
            if (parameters.Count > 2)
                return false;
            foreach (string parameter in parameters)
            {
                bool result = int.TryParse(parameter, out int _result);
                if (result == false)
                    return false;
            }
            if (parameters.Count == 2)
            {
                int a = int.Parse(parameters[0]);
                int b = int.Parse(parameters[1]);
                if (a > b)
                    return false;
            }
            return true;
        }

        private List<int> ConvertStringToInt(List<string> parameters)
        {
            List<int> _parameters = new List<int>();
            foreach (string parameter in parameters)
            {
                int _parameter = int.Parse(parameter);
                _parameters.Add(_parameter);
            }
            return _parameters;
        }

        private int Generate(int min = 0, int max = 100)
        {
            return random.Next(min, max);
        }
    }
}
