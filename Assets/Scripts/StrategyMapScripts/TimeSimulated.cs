
public class TimeSimulated
{
    public double hour;
    public double day;
    public double week;
    public double month;
    public double year;
    public string season;

    public TimeSimulated( double _day, double _month, double _year)
    {
        this.hour = 0f;
        this.day = _day;
        this.month = _month;
        this.year = _year;
        CalculateWeeks();
        CalculateSeason();
    }
    public void CalculateSeason()
    {
        if (month == 12 || month == 1 || month == 2)
        {
            season = "Temporada 1";
        }
        if (month == 3 || month == 4 || month == 5)
        {
            season= "Temporada 2";
        }
        if (month == 6 || month == 7 || month == 8)
        {
            season = "Temporada 3";
        }
        if (month == 9 || month == 10 || month == 11)
        {
            season = "Temporada 4";
        }
    }
    public string PrintTimeSimulated()
    {
        return this.day.ToString() + " / " + this.month.ToString() + " / " + this.year.ToString();
    }
    public void CalculateWeeks()
    {
        double total_days = day + 30 * (month - 1)-1;
        week = (int)(total_days / 10) + 1;
    }
    public void CalculateYear()
    {
        year++;
        month = 1;
    }
    public void CalculateMonth()
    {
        month++;
        day = 1;
    }
    public void CalculateDay()
    {
        day++;
        hour = 1;
    }
    public void PlusDays(int plus)
    {
        day += plus;
    }
    public bool EqualsDate(TimeSimulated time2)
    {
        return this.day == time2.day && this.month == time2.month && this.year == time2.year;
    }

}
