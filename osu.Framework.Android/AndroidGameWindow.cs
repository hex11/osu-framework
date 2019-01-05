﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Configuration;
using osu.Framework.Platform;
using osuTK.Graphics;

namespace osu.Framework.Android
{
    public class AndroidGameWindow : GameWindow
    {
        public override IGraphicsContext Context
            => Implementation.GraphicsContext;

        protected new AndroidGameView Implementation
            => base.Implementation as AndroidGameView;

        public override bool Focused
            => true;

        public override osuTK.WindowState WindowState {
            get => osuTK.WindowState.Normal;
            set { }
        }

        public AndroidGameWindow(AndroidGameView view) : base(view)
        {
        }

        public override void SetupWindow(FrameworkConfigManager config)
        {
        }

        public override void Run()
        {
        }

        public override void Run(double updateRate)
        {
        }
    }
}
