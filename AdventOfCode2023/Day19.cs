namespace AdventOfCode2023;

public class Day19 : BaseDay
{
    private const string START_WORKFLOW = "in";
    private const string ACCEPT_TARGET = "A";
    private const string REJECT_TARGET = "R";

    private readonly Dictionary<string, Workflow> _workflows;
    private readonly List<Part> _parts;

    public Day19()
    {
        (_workflows, _parts) = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        _parts.Where(ProcessPart)
              .Sum(p => p.TotalRating)
              .ToString()
    );

    public override ValueTask<string> Solve_2()
    {
        var possibleParts = new Queue<PossibleParts>();
        possibleParts.Enqueue(new PossibleParts(START_WORKFLOW, new() { { 'x', new(1, 4000) }, { 'm', new(1, 4000) }, { 'a', new(1, 4000) }, { 's', new(1, 4000) } }));

        var acceptedParts = new List<PossibleParts>();

        while (possibleParts.TryDequeue(out var current))
        {
            foreach (var rule in _workflows[current.Workflow].Rules)
            {
                if (rule is OtherwiseRule)
                {
                    if (rule.Target == ACCEPT_TARGET)
                    {
                        acceptedParts.Add(current);
                    }
                    else if (rule.Target != REJECT_TARGET)
                    {
                        possibleParts.Enqueue(new PossibleParts(rule.Target, current.Ratings));
                    }
                }
                else if (rule is ConditionalRule condition)
                {
                    if (condition.Comparison == '<')
                    {
                        if (current.Ratings[condition.Rating].End < condition.CompareValue)
                        {
                            if (rule.Target == ACCEPT_TARGET)
                            {
                                acceptedParts.Add(current);
                            }
                            else if (rule.Target != REJECT_TARGET)
                            {
                                possibleParts.Enqueue(new PossibleParts(condition.Target, current.Ratings));
                            }
                        }
                        else if (current.Ratings[condition.Rating].Start < condition.CompareValue)
                        {
                            var split = current.Duplicate();
                            split.Ratings[condition.Rating] = new(split.Ratings[condition.Rating].Start, condition.CompareValue - 1);
                            current.Ratings[condition.Rating] = new(condition.CompareValue, current.Ratings[condition.Rating].End);
                            if (rule.Target == ACCEPT_TARGET)
                            {
                                acceptedParts.Add(split);
                            }
                            else if (rule.Target != REJECT_TARGET)
                            {
                                possibleParts.Enqueue(new PossibleParts(condition.Target, split.Ratings));
                            }
                        }
                    }
                    else if (condition.Comparison == '>')
                    {
                        if (current.Ratings[condition.Rating].Start > condition.CompareValue)
                        {
                            if (rule.Target == ACCEPT_TARGET)
                            {
                                acceptedParts.Add(current);
                            }
                            else if (rule.Target != REJECT_TARGET)
                            {
                                possibleParts.Enqueue(new PossibleParts(condition.Target, current.Ratings));
                            }
                        }
                        else if (current.Ratings[condition.Rating].End > condition.CompareValue)
                        {
                            var split = current.Duplicate();
                            split.Ratings[condition.Rating] = new(condition.CompareValue + 1, split.Ratings[condition.Rating].End);
                            current.Ratings[condition.Rating] = new(current.Ratings[condition.Rating].Start, condition.CompareValue);
                            if (rule.Target == ACCEPT_TARGET)
                            {
                                acceptedParts.Add(split);
                            }
                            else if (rule.Target != REJECT_TARGET)
                            {
                                possibleParts.Enqueue(new PossibleParts(condition.Target, split.Ratings));
                            }
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        return new(
            acceptedParts.Sum(p => p.Combinations()).ToString()
        ); 
    }

    private bool ProcessPart(Part part)
    {
        var current = START_WORKFLOW;

        while (current != ACCEPT_TARGET && current != REJECT_TARGET)
        {
            var workflow = _workflows[current];
            current = workflow.ProcessPart(part);
        }

        return current == ACCEPT_TARGET;
    }

    private record class Part(int X, int M, int A, int S)
    {
        public int TotalRating => X + M + A + S;
        public int Get(char rating) => rating switch
        {
            'x' => X,
            'm' => M,
            'a' => A,
            's' => S,
            _ => throw new ArgumentException($"Invalid rating {rating}")
        };
    }

    private record class PossibleParts(string Workflow, Dictionary<char, Range<int>> Ratings)
    {
        public long Combinations() => Ratings.Values.Product(r => 1L * r.Length);

        public PossibleParts Duplicate()
            => new(Workflow, Ratings.ToDictionary(kv => kv.Key, kv => kv.Value));
    }

    private interface IRule
    {
        public string Target { get; }
        public bool Matches(Part part);
    }

    private record class ConditionalRule(char Rating, char Comparison, int CompareValue, string Target) : IRule
    {
        public bool Matches(Part part) => Comparison switch
        {
            '<' => part.Get(Rating) < CompareValue,
            '>' => part.Get(Rating) > CompareValue,
            _ => throw new ArgumentException($"Invalid comparison {Comparison}")
        };
    }

    private record class OtherwiseRule(string Target) : IRule
    {
        public bool Matches(Part part) => true;
    }

    private record class Workflow(string Name, List<IRule> Rules)
    {
        public string ProcessPart(Part part) => Rules.First(rule => rule.Matches(part)).Target;
    }

    private (Dictionary<string, Workflow>, List<Part>) ParseInput()
    {
        var lines = AocDownloader.GetInput(2023, 19).SplitIntoLines();

        var workflows = lines
            .TakeWhile(line => !string.IsNullOrEmpty(line))
            .Select(line =>
                {
                    var items = line.Split(['{', ',', '}']);
                    var rules = items[1..^1].Select(ParseRule).ToList();
                    return new Workflow(items[0], rules);
                })
            .ToDictionary(workflow => workflow.Name);

        var parts = lines
            .Select(ParsePart)
            .ToList();

        return (workflows, parts);
    }

    private IRule ParseRule(string input)
    {
        if (!input.Contains(':'))
        {
            return new OtherwiseRule(input);
        }

        var items = input.Split(':');

        return new ConditionalRule(items[0][0], items[0][1], int.Parse(items[0][2..]), items[1]);
    }

    private Part ParsePart(string input)
    {
        var items = input.Split(['{', ',', '}'])[1..^1].Select(x => int.Parse(x[2..])).ToArray();
        return new Part(items[0], items[1], items[2], items[3]);
    }
}
