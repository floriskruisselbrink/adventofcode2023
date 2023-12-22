using System.Diagnostics;
using System.Xml.XPath;

namespace AdventOfCode2023;

public class Day20 : BaseDay
{
    private readonly string[] _input;

    public Day20()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var modules = SetupModules();
        var actions = new Queue<Action>();
        var pulses = new Dictionary<Pulse, long> { { Pulse.Low, 0L }, { Pulse.High, 1L } };

        void PressButton()
        {
            actions.Enqueue(new Action("button", Pulse.Low, "broadcaster"));
            pulses[Pulse.Low]++;

            while (actions.TryDequeue(out var action))
            {
                if (modules.TryGetValue(action.Destination, out var module))
                {
                    var result = module.ProcessPulse(action.Source, action.Pulse);

                    if (result != Pulse.Nothing)
                    {
                        foreach (var destination in module.Destinations)
                        {
                            actions.Enqueue(new Action(action.Destination, result, destination));
                            pulses[result]++;
                        }
                    }
                }
            }
        }

        for (int i=0; i < 1000; i++)
        {
            PressButton();
        }

        return new(pulses.Values.Product().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var modules = SetupModules();
        var moduleBeforeRx = (Conjunction)modules.Where(m => m.Value.Destinations.Contains("rx")).First().Value;

        var actions = new Queue<Action>();
        var cycles = moduleBeforeRx.Received.Keys.ToDictionary(m => m, _ => 0L);

        var buttonPresses = 0L;

        while (true)
        {
            actions.Enqueue(new Action("button", Pulse.Low, "broadcaster"));
            buttonPresses++;

            while (actions.TryDequeue(out var action))
            {
                if (modules.TryGetValue(action.Destination, out var module))
                {
                    var result = module.ProcessPulse(action.Source, action.Pulse);

                    if (result != Pulse.Nothing)
                    {
                        foreach (var destination in module.Destinations)
                        {
                            if (result == Pulse.High && destination == moduleBeforeRx.Name && cycles[module.Name] == 0)
                            {
                                cycles[module.Name] = buttonPresses;
                                if (cycles.All(cycle => cycle.Value > 0))
                                {
                                    return new(cycles.Values.Product().ToString());
                                }
                            }

                            actions.Enqueue(new Action(action.Destination, result, destination));
                        }
                    }
                }
            }
        }

    }

    public enum Pulse
    {
        Low,
        Nothing,
        High
    }

    public record struct Action(string Source, Pulse Pulse, string Destination);

    private abstract record class Module(string Name, IList<string> Destinations)
    {
        abstract public Pulse ProcessPulse(string source, Pulse pulse);
    }

    private record class FlipFlop(string Name, IList<string> Destinations) : Module(Name, Destinations)
    {
        private bool _state = false;

        public override Pulse ProcessPulse(string source, Pulse pulse)
        {
            if (pulse == Pulse.High)
            {
                return Pulse.Nothing;
            }

            if (_state)
            {
                _state = false;
                return Pulse.Low;
            }
            else
            {
                _state = true;
                return Pulse.High;
            }
        }
    }

    private record class Conjunction(string Name, IList<string> Destinations) : Module(Name, Destinations)
    {
        public IDictionary<string, Pulse> Received { get; } = new Dictionary<string, Pulse>();

        public override Pulse ProcessPulse(string source, Pulse pulse)
        {
            Debug.Assert(Received.Count > 0);

            Received[source] = pulse;

            if (Received.All(r => r.Value == Pulse.High))
            {
                return Pulse.Low;
            }
            else
            {
                return Pulse.High;
            }
        }

        public void ClearState(IEnumerable<string> inputs)
        {
            Received.Clear();
            foreach (var input in inputs)
            {
                Received[input] = Pulse.Low;
            }
        }
    }

    private record class Broadcast(IList<string> Destinations) : Module("broadcaster",Destinations)
    {
        public override Pulse ProcessPulse(string source, Pulse pulse)
        {
            return pulse;
        }
    }

    private Dictionary<string, Module> SetupModules()
    {
        var modules = new Dictionary<string, Module>();

        foreach (var line in _input)
        {
            var items = line.Split(" -> ");
            string name = items[0];
            string[] destinations = items[1].Split(", ");
            Module module = name[0] switch
            {
                '%' => new FlipFlop(name[1..], destinations),
                '&' => new Conjunction(name[1..], destinations),
                _ => new Broadcast(destinations)
            };
            modules[module.Name] = module;
        }

        foreach (var (name, module) in modules.Where(m => m.Value is Conjunction))
        {
            var inputs = modules.Where(m => m.Value.Destinations.Contains(name)).Select(m => m.Key);
            ((Conjunction)module).ClearState(inputs);
        }
        return modules;
    }

    private string[] ParseInput()
        => AocDownloader.GetInput(2023, 20)
        .SplitIntoLines().ToArray();
}
