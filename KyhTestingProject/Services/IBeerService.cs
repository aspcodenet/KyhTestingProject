namespace KyhTestingProject.Services;
public interface IBeerService
{
    bool CanIBuyBeer(int age, Location location, decimal promilleHalt);
}
public enum Location
{
    Krogen,
    Systemet
}
public class SwedishBeerService : IBeerService
{
    public bool CanIBuyBeer(int age, Location location, decimal promilleHalt)
    {
        if (promilleHalt > 1.0m) 
            return false;
        if (age >= 18 && location == Location.Krogen) 
            return true;
        if (age >= 20 && location == Location.Systemet) 
            return true;
        return false;
    }
}
public class NorwegianBeerService : IBeerService
{
    public bool CanIBuyBeer(int age, Location location, decimal promilleHalt)
    {
        if (promilleHalt > 1.5m) return false;
        if (age >= 18 && location == Location.Krogen) return true;
        if (age >= 20 && location == Location.Systemet) return true;
        return false;
    }
}


