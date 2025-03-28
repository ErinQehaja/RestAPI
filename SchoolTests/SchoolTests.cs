using System;
using System.Collections.Generic;
using MyFirstWebApplication.Class;
using NUnit.Framework;
using MyFirstWebApplication;

public class SchoolTests
{
    [Test]
    public void GetTotalStudents_AddStudent_IncreasesTotalStudents()
    {
        var school = new School("Test School");
        var student = new Student("Male", new DateTime(2007, 4, 14), "1A");

        school.AddStudent(student);

        Assert.AreEqual(1, school.GetTotalStudents());
    }

    [Test]
    public void GetStudentsByGender_TwoDifferentGenders_ReturnsCorrectCount()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "1A"));

        Assert.AreEqual(1, school.GetStudentsByGender("Male"));
        Assert.AreEqual(1, school.GetStudentsByGender("Female"));
    }

    [Test]
    public void GetTotalClassrooms_AddTwoClassrooms_ReturnsCorrectCount()
    {
        var school = new School("Test School");
        school.AddClassroom(new Classroom("1A", 50.0, 30, true));
        school.AddClassroom(new Classroom("2B", 60.0, 25, false));

        Assert.AreEqual(2, school.GetTotalClassrooms());
    }

    [Test]
    public void GetAverageAge_ReturnsCorrectValue()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "1A"));

        var today = DateTime.Today;
        double expectedAverageAge = (today - new DateTime(2007, 4, 14)).TotalDays / 365 +
                                    (today - new DateTime(2006, 6, 23)).TotalDays / 365;
        expectedAverageAge /= 2.0;

        Assert.AreEqual(expectedAverageAge, school.GetAverageAge(), 0.01);
    }

    [Test]
    public void GetClassroomsWithCynap_AddTwoRoomsWithAndWithoutCynap_ReturnsCorrectRooms()
    {
        var school = new School("Test School");
        var roomWithCynap = new Classroom("1A", 50.0, 30, true);
        var roomWithoutCynap = new Classroom("2B", 60.0, 25, false);

        school.AddClassroom(roomWithCynap);
        school.AddClassroom(roomWithoutCynap);

        var result = school.GetClassroomsWithCynap();

        Assert.AreEqual(1, result.Count);
        Assert.Contains(roomWithCynap, result);
    }

    [Test]
    public void GetTotalClasses_AddTwoClasses_ReturnsCorrectCount()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "2B"));

        Assert.AreEqual(2, school.GetTotalClasses());
    }

    [Test]
    public void GetClassesWithStudentCount_AddThreeStudents_ReturnsCorrectData()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "1A"));
        school.AddStudent(new Student("Male", new DateTime(2005, 5, 15), "2B"));

        var result = school.GetClassesWithStudentCount();

        Assert.AreEqual(2, result["1A"]);
        Assert.AreEqual(1, result["2B"]);
    }

    [Test]
    public void GetFemalePercentageInClass_AddOneFemale_ReturnsCorrectValue()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "1A"));

        double result = school.GetFemalePercentageInClass("1A");

        Assert.AreEqual(50.0, result);
    }

    [Test]
    public void CanClassFitInRoom_AddTwoStudentsOneClassroom_ReturnsCorrectResult()
    {
        var school = new School("Test School");
        school.AddStudent(new Student("Male", new DateTime(2007, 4, 14), "1A"));
        school.AddStudent(new Student("Female", new DateTime(2006, 6, 23), "1A"));

        var room = new Classroom("1A", 50.0, 2, true);
        school.AddClassroom(room);

        Assert.IsTrue(school.CanClassFitInRoom("1A", "1A"));
        Assert.IsFalse(school.CanClassFitInRoom("1A", "NonExistentRoom"));
    }
}