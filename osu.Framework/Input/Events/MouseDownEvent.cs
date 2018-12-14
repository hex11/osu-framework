﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using osu.Framework.Input.States;
using osuTK;
using osuTK.Input;

namespace osu.Framework.Input.Events
{
    /// <summary>
    /// An event representing a press of a mouse button.
    /// </summary>
    public class MouseDownEvent : MouseButtonEvent
    {
        public MouseDownEvent(InputState state, MouseButton button, Vector2? screenSpaceMouseDownPosition = null)
            : base(state, button, screenSpaceMouseDownPosition)
        {
        }
    }
}