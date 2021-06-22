using UnityEngine;
[System.Serializable]
public class TimeSimulated
{
    [SerializeField] private double day;
    [SerializeField] private double month;
    [SerializeField] private double year;
    [SerializeField] private double hour;
    private double week;
    private string season;
    public double Hour
    {
        get { return hour; }
        set { hour = value; }
    }
    public double Day
    {
        get { return day; }
        set { day = value; }
    }
    public double Week
    {
        get { return week; }
        set { week = value; }
    }
    public double Month
    {
        get { return month; }
        set { month = value; }
    }
    public double Year
    {
        get { return year; }
        set { year = value; }
    }
    public string Season
    {
        get { return season; }
        set { season = value; }
    }
    public TimeSimulated()
    {

    }
    public TimeSimulated(TimeSimulated _timeSimulated)
    {
        this.hour = 0f;
        this.day = _timeSimulated.day;
        this.month = _timeSimulated.month;
        this.year = _timeSimulated.year;
        CalculateWeeks();
        CalculateSeason();
    }
    public TimeSimulated( double _day, double _month, double _year)
    {
        this.hour = 0f;
        this.day = _day;
        this.month = _month;
        this.year = _year;
        CalculateWeeks();
        CalculateSeason();
    }
    /// <summary>
    /// Season of a TimeSimulated
    /// </summary>
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
    /// <summary>
    /// Number weeks of a TimeSimulated
    /// </summary>
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

    /// <summary>
    /// Add plus days to a TimeSimulated
    /// </summary>
    /// <param name="plus"></param>
    public void PlusDays(int plus)
    {
        day += plus;
        if (this.day > 30)
        {
            int a = (int)this.day / 30;
            month += a;
            this.day = (int)this.day % 30; 
        }
        if (this.month > 12)
        {
            int b = (int)this.month - 12;
            this.month = b;
            year++;
        }
    }
    /// <summary>
    /// Add plus month to a TimeSimulated
    /// </summary>
    /// <param name="plus"></param>
    public void PlusMonth(int plus)
    {
        month += plus;
        if (this.month > 12)
        {
            int b = (int)this.month - 12;
            this.month = b;
            year++;
        }
    }
    /// <summary>
    /// Returns if two time simulated are equals
    /// </summary>
    /// <param name="time2"></param>
    /// <returns></returns>
    public bool EqualsDate(TimeSimulated time2)
    {
        return this.hour == time2.hour && this.day == time2.day && this.month == time2.month && this.year == time2.year;
    }

    /// <summary>
    /// Returns days from two time simulated
    /// </summary>
    /// <param name="time2"></param>
    /// <returns></returns>
    public int DiferenceDays(TimeSimulated time2)
    {
        
        int difYear = (int)(this.year - time2.year);
        int difMonth =(int)(this.month - time2.month);
        int difDays = (int)(this.day - time2.day);
//        int hour = (int)(this.hour - time2.hour);
        return difYear*360 + difMonth * 30 + difDays;
    }
}
