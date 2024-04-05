using System;
using System.Collections.Generic;
using System.Linq;

public enum RoomStyle
{
    TwinRoom,
    QueenRoom,
    KingRoom
}

public class HotelRoom
{
    public int RoomNumber { get; set; }
    public RoomStyle Style { get; set; }
    public bool Status { get; set; }
    public decimal BookingPrice { get; set; }

    public HotelRoom(int roomNumber, RoomStyle style, decimal bookingPrice)
    {
        RoomNumber = roomNumber;
        Style = style;
        Status = true;
        BookingPrice = bookingPrice;
    }

    public override string ToString()
    {
        return $"Room {RoomNumber}, Style: {Style.ToString().Replace("Room", " Room")}, Price: {BookingPrice}";
    }
}

public class Hotel
{
    public string HotelName { get; set; }
    public string Location { get; set; }
    public List<HotelRoom> AllRooms { get; set; }

    public Hotel(string hotelName, string location, List<HotelRoom> allRooms)
    {
        HotelName = hotelName;
        Location = location;
        AllRooms = allRooms;
    }

    public void DisplayAvailableRooms()
    {
        Console.WriteLine($"\n{HotelName} - Available Rooms");
        foreach (var room in AllRooms.Where(r => r.Status))
        {
            Console.WriteLine(room.ToString());
        }
    }

    public void DisplayBookedRooms()
    {
        Console.WriteLine($"\n{HotelName} - Booked Rooms");
        foreach (var room in AllRooms.Where(r => !r.Status))
        {
            Console.WriteLine(room.ToString());
        }
    }
}

public class Guest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public long PhoneNumber { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Guest(string name, string address, string email, long phoneNumber)
    {
        Name = name;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        Reservations = new List<Reservation>();
    }

    public void DisplayReservations()
    {
        Console.WriteLine($"\nList of Reservations of {Name}:");
        foreach (var reservation in Reservations)
        {
            Console.WriteLine(reservation.ToString());
        }
    }
}

public class Reservation
{
    private static int _reservationNumberCounter = 1234567890;
    public int ReservationNumber { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public HotelRoom Room { get; set; }
    public int DurationInDays => (int)(EndTime - StartTime).TotalDays;
    public decimal Total => Room.BookingPrice * DurationInDays;

    public Reservation(DateTime startTime, DateTime endTime, HotelRoom room)
    {
        ReservationNumber = ++_reservationNumberCounter;
        StartTime = startTime;
        EndTime = endTime;
        Room = room;
        room.Status = false;
    }

    public override string ToString()
    {
        return $"{ReservationNumber} Start Time: {StartTime.ToString("dd/MM/yyyy hh:mm:ss tt")}, End Time: {EndTime.ToString("dd/MM/yyyy hh:mm:ss tt")}, Duration: {DurationInDays}, Total: {Total}";
    }
}

public class Receptionist
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public long PhoneNumber { get; set; }

    public Receptionist(string name, string address, string email, long phoneNumber)
    {
        Name = name;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public void BookReservation(Guest guest, Reservation reservation)
    {
        guest.Reservations.Add(reservation);
    }
}

public class HotelManagementSystem
{
    private List<Hotel> hotels;
    private List<Guest> users;
    private List<Reservation> reservations;
    private List<Receptionist> receptionists;

    public HotelManagementSystem()
    {
        hotels = new List<Hotel>();
        users = new List<Guest>();
        reservations = new List<Reservation>();
        receptionists = new List<Receptionist>();
    }

    public void RegisterUser(Guest user)
    {
        users.Add(user);
    }

    public void RegisterReceptionist(Receptionist receptionist)
    {
        receptionists.Add(receptionist);
    }

    public void AddHotel(Hotel hotel)
    {
        hotels.Add(hotel);
    }

    public void DisplayHotels()
    {
        Console.WriteLine("List of Hotels:");
        foreach (var hotel in hotels)
        {
            Console.WriteLine($"{hotel.HotelName}, {hotel.Location}");
        }
    }

    public void BookReservation(Hotel hotel, HotelRoom room, Guest guest, DateTime startTime, DateTime endTime)
    {
        Reservation reservation = new Reservation(startTime, endTime, room);
        reservations.Add(reservation);
        guest.Reservations.Add(reservation);
    }

    public void BookReservation(Receptionist receptionist, Guest guest, Reservation reservation)
    {
        receptionist.BookReservation(guest, reservation);
        reservations.Add(reservation);
    }

    public void DisplayReservationDetails(int reservationNumber)
    {
        var reservation = reservations.FirstOrDefault(r => r.ReservationNumber == reservationNumber);
        if (reservation != null)
        {
            Console.WriteLine(reservation.ToString());
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<HotelRoom> yananRooms = new List<HotelRoom>();
        HotelRoom room1 = new HotelRoom(101, RoomStyle.TwinRoom, 1500);
        HotelRoom room2 = new HotelRoom(102, RoomStyle.KingRoom, 3000);
        yananRooms.Add(room1);
        yananRooms.Add(room2);
        Hotel hotelYanan = new Hotel("Hotel Yanan", "123 GStreet, Takaw City", yananRooms);

        List<HotelRoom> hotel456Rooms = new List<HotelRoom>();
        HotelRoom hotel456Room1 = new HotelRoom(101, RoomStyle.QueenRoom, 2000);
        HotelRoom hotel456Room2 = new HotelRoom(102, RoomStyle.QueenRoom, 2000);
        hotel456Rooms.Add(hotel456Room1);
        hotel456Rooms.Add(hotel456Room2);
        Hotel hotel456 = new Hotel("Hotel 456", "Session Road, Baguio City", hotel456Rooms);

        HotelManagementSystem hms = new HotelManagementSystem();
        hms.AddHotel(hotelYanan);
        hms.AddHotel(hotel456);

        hms.DisplayHotels();

        hotelYanan.DisplayAvailableRooms();

        Guest terry = new Guest("Terry", "Addr 1", "terry@email.com", 63919129);
        hms.RegisterUser(terry);

        hms.BookReservation(hotelYanan, room1, terry, DateTime.Now, new DateTime(2024, 04, 16));

        hotelYanan.DisplayBookedRooms();

        terry.DisplayReservations();

        Receptionist anna = new Receptionist("Anna", "Addr 2", "anna@email.com", 67890);
        hms.RegisterReceptionist(anna);

        Reservation res = new Reservation(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06), hotel456Room2);
        hms.BookReservation(anna, terry, res);

        terry.DisplayReservations();

        hms.DisplayReservationDetails(1234567890); //
    }
}