using System.Drawing;

namespace Conquer.Game.Models;

public sealed class Weather
{
    private const int MaxIntensity = 1000;
    private const int Spread = MaxIntensity / 2;
    private const int FineChance = 60;
    private const int TurnChance = 5;
    private const int ShortestSpell = 500;
    private const int LongestSpell = 1200;
    private const int CloudySpells = 5;

    private readonly WeatherType _usual;
    private readonly int _usualIntensity;
    private readonly Color _color;
    private WeatherType _type;
    private int _intensity;
    private int _direction;
    private DateTimeOffset _changes;

    public Weather(Region region, Random random, DateTimeOffset now)
    {
        var (type, intensity) = ((WeatherType)region.Data0, (int)region.Data1);
        (_usual, _usualIntensity) = type == WeatherType.None || intensity == 0 ? (WeatherType.None, 0) : (type, intensity);
        _type = _usual;
        _intensity = _usualIntensity;
        _direction = (int)region.Data2;
        _color = Color.FromArgb((int)region.Data3);
        _changes = now + Spell(random);
    }

    public MsgWeather Message => new()
    {
        WeatherType = _type,
        Intensity = Particles(),
        Direction = (uint)_direction,
        Color = _color
    };

    public bool Tick(Random random, DateTimeOffset now)
    {
        if (_usual == WeatherType.None || now < _changes)
        {
            return false;
        }

        var before = (_type, Particles());
        var type = random.Next(100) < FineChance ? WeatherType.None : _usual;

        if (type != _type && random.Next(100) < TurnChance)
        {
            _direction = random.Next(360);
        }

        _intensity = type == WeatherType.None ? 0 : Math.Clamp(_usualIntensity - Spread / 2 + random.Next(Spread), 1, MaxIntensity - 1);
        _type = type;
        _changes = now + Spell(random);
        return (_type, Particles()) != before;
    }

    private TimeSpan Spell(Random random) =>
        TimeSpan.FromSeconds(random.Next(ShortestSpell, LongestSpell + 1) * (_type == WeatherType.Cloudy ? CloudySpells : 1));

    private uint Particles()
    {
        var particles = Math.Min(_intensity + (int)Math.Sqrt(MaxIntensity), MaxIntensity - 1);
        particles = particles * particles / MaxIntensity;

        return (uint)(_type switch
        {
            WeatherType.None => 0,
            WeatherType.Rain or WeatherType.Sand => (particles + 1) / 2,
            WeatherType.Snow => (particles + 3) / 4,
            WeatherType.Atoms => (particles + 29) / 30,
            WeatherType.Cloudy => (particles + 33) / 34,
            _ => (particles + 24) / 25
        });
    }
}
