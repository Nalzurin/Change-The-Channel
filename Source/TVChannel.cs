using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ChangeTheChannel
{
    public class EventTraitThought : IExposable
    {
        public TraitDef traitDef;
        public int degree = 0;
        public ThoughtDef thoughtDef;
        public EventTraitThought() { }
        public void ExposeData()
        {
            Scribe_Values.Look(ref degree, "degree");
            Scribe_Defs.Look(ref traitDef, "traitDef");
            Scribe_Defs.Look(ref thoughtDef, "thoughtDef");

        }
    }
    public class TVChannelEvent : IExposable
    {
        public string text;
        public List<EventTraitThought> eventTraitThoughts;
        public TVChannelEvent() { }
        public void ExposeData()
        {
            Scribe_Values.Look(ref text, "text");
            Scribe_Collections.Look(ref eventTraitThoughts, "eventTraitThoughts", LookMode.Deep);
        }
    }
    public class TVChannel : IExposable
    {
        public string title;
        public SkillDef skill = null;
        public List<TVChannelEvent> events = [];
        private List<TVChannelEvent> playedEvents = [];
        public TVChannel() { }
        public TVChannelEvent GetRandomEvent()
        {
            TVChannelEvent tvEvent = events.RandomElement();
            playedEvents.Add(tvEvent);
            events.Remove(tvEvent);
            if (events.Empty())
            {
                events = [.. playedEvents];
                playedEvents = [];
            }
            return tvEvent;
        }
        public void ExposeData()
        {
            Scribe_Values.Look(ref title, "title");
            Scribe_Defs.Look(ref skill, "skill");
            Scribe_Collections.Look(ref events, "events", LookMode.Deep);
            Scribe_Collections.Look(ref playedEvents, "playedEvents", LookMode.Deep);

        }
    }
}
