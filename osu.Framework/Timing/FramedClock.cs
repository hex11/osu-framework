﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Extensions.TypeExtensions;
using osu.Framework.MathUtils;
using System;
using System.Diagnostics;

namespace osu.Framework.Timing
{
    /// <summary>
    /// Takes a clock source and separates time reading on a per-frame level.
    /// The CurrentTime value will only change on initial construction and whenever ProcessFrame is run.
    /// </summary>
    public class FramedClock : IFrameBasedClock
    {
        public IClock Source { get; }

        /// <summary>
        /// Construct a new FramedClock with an optional source clock.
        /// </summary>
        /// <param name="source">A source clock which will be used as the backing time source. If null, a StopwatchClock will be created. When provided, the CurrentTime of <see cref="source" /> will be transferred instantly.</param>
        public FramedClock(IClock source = null)
        {
            if (source != null)
            {
                CurrentTime = LastFrameTime = source.CurrentTime;
                Source = source;
            }
            else
                Source = new StopwatchClock(true);
        }

        public FrameTimeInfo TimeInfo => new FrameTimeInfo { Elapsed = ElapsedFrameTime, Current = CurrentTime };

        public double AverageFrameTime { get; private set; }

        public double FramesPerSecond { get; private set; }

        public virtual double CurrentTime { get; protected set; }

        protected virtual double LastFrameTime { get; set; }

        public double Rate => Source.Rate;

        protected double SourceTime => Source.CurrentTime;

        public double ElapsedFrameTime => CurrentTime - LastFrameTime;

        public bool IsRunning => Source?.IsRunning ?? false;

        private double timeUntilNextCalculation;
        private double timeSinceLastCalculation;
        private int framesSinceLastCalculation;

        /// <summary>
        /// Whether we should run <see cref="ProcessFrame"/> on the underlying <see cref="Source"/> (in the case it is an <see cref="IFrameBasedClock"/>).
        /// </summary>
        public bool ProcessSourceClockFrames = true;

        private const int fps_calculation_interval = 250;

        public virtual void ProcessFrame()
        {
            if (ProcessSourceClockFrames)
                (Source as IFrameBasedClock)?.ProcessFrame();

            if (timeUntilNextCalculation <= 0)
            {
                timeUntilNextCalculation += fps_calculation_interval;

                if (framesSinceLastCalculation == 0)
                    FramesPerSecond = 0;
                else
                    FramesPerSecond = (int)Math.Ceiling(framesSinceLastCalculation * 1000f / timeSinceLastCalculation);
                timeSinceLastCalculation = framesSinceLastCalculation = 0;
            }

            framesSinceLastCalculation++;
            timeUntilNextCalculation -= ElapsedFrameTime;
            timeSinceLastCalculation += ElapsedFrameTime;

            AverageFrameTime = Interpolation.Damp(AverageFrameTime, ElapsedFrameTime, 0.01, ElapsedFrameTime / 1000);

            LastFrameTime = CurrentTime;
            CurrentTime = SourceTime;

            // Small optimization to prevent the string from being allocated every frame.
            if (ElapsedFrameTime < 0)
                Trace.Assert(ElapsedFrameTime >= 0, $"Time should not run backwards, but did. {nameof(CurrentTime)}={CurrentTime} {nameof(LastFrameTime)}={LastFrameTime}");
        }

        public override string ToString() => $@"{GetType().ReadableName()} ({Math.Truncate(CurrentTime)}ms, {FramesPerSecond} FPS)";
    }
}
