﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibraryCore.Helpers
{
    public sealed class AsyncStatus
    {
        DateTime StartTime;
        int TimesRun;
        int UpdateFrequency;
        public double RunAverage { get; private set; }
        public object Dependant { get; private set; }
        public Task RequestedTask { get; private set; }
        public CancellationTokenSource TokenSrc { get; private set; }

        public AsyncStatus(object dependant, int frequency)
        {
            TokenSrc = new CancellationTokenSource();
            StartTime = DateTime.Now;
            Dependant = dependant;
            UpdateFrequency = frequency;
            // technically 0 but it's faster than checking for division by 0
            TimesRun = 1;
        }

        public CancellationToken GetToken()
        {
            return TokenSrc.Token;
        }

        public double ElapsedMillisecondsTime()
        {
            return (DateTime.Now - StartTime).TotalMilliseconds;
        }

        public void Update(Task<bool> T)
        {
            // reset the token source
            TokenSrc.Dispose();
            TokenSrc = new CancellationTokenSource();

            RequestedTask = T;
           // Console.WriteLine($"Starting Task {T.Id} ");
            RequestedTask.Start();

            if (TimesRun > 25)
                TimesRun = 1;

            RunAverage = RunAverage + ((DateTime.Now - StartTime).TotalMilliseconds - RunAverage - UpdateFrequency) / TimesRun;
            StartTime = DateTime.Now;
        }

        public void Abort()
        {
            RequestedTask = null;
        }
    }
}
