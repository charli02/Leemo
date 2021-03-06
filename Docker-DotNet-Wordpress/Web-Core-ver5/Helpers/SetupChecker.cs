using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Core_ver5.Helpers
{
    public class SetupChecker
    {
        private readonly Random _random;
        private int index;

        private readonly string[] Status =
            {"Grinding beans", "Steaming milk", "Taking a sip (quality control)", "On transit to counter", "Picked up"};

        public SetupChecker(Random random)
        {
            _random = random;
        }

        public CheckResult GetUpdate(int orderNo)
        {
            if (_random.Next(1, 5) == 4)
            {
                if (Status.Length - 1 > index)
                {
                    index++;
                    var result = new CheckResult
                    {
                        New = true,
                        Update = Status[index],
                        Finished = Status.Length - 1 == index
                    };
                    if (result.Finished)
                        index = 0;
                    return result;
                }
            }

            return new CheckResult { New = false };
        }
    }

    public class CheckResult
    {
        public bool New { get; set; }
        public string Update { get; set; }
        public bool Finished { get; set; }
    }
}
