﻿using Sol.Domain.Utilities;

namespace Sol.Test.TestDoubles
{
    public class TestExporter : IExporter
    {
        public int Calls { get; private set; } = 0;

        public void Export(string directory, string fileName, Func<string> serializeFunc)
        {
            Calls++;
        }
    }
}
