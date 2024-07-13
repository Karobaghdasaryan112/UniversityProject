    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
namespace UniversityProject
{
    // himnakan
    public class University
    {
        public string Name;
        public DateTime DateTime;
        public Faculty faculty;
        public int FacultyCount;
        public int StudentsCount;
        private int MaxStudentCount = 1000;
        public List<string> FacultiesNames = new List<string> { " Information Technology", "Engineering", "Physics", "Mathematics" };
        public List<string> SubjectAdmissions = new List<string> { "Mathematics", "Physics", "computer science", "English" };
        public List<Faculty> faculties = new List<Faculty>();
        public List<Student> Students = new List<Student>();
        public List<string>[] SubjectsOfFaculty = new List<string>[]
        {
              new List<string>{"Programming", "Computer Networks", "Databases", "Web Development", "Cybersecurity"},
              new List<string>{"Mechanics", "Electrical Engineering", "Materials Science", "Thermodynamics", "Control Systems"},
              new List<string>{"Quantum Mechanics", "Optics", "Astrophysics", "Nuclear Physics", "Thermodynamics"},
              new List<string>{"Differential Equations", "Linear Algebra", "Probability Theory", "Mathematical Analysis", "Topology"}
        };
        public List<string> GetSubjectsOfFaculty(int index)
        {
            return SubjectsOfFaculty[index];
        }
        public List<string> GetSubjectsAdmissions()
        {
            Random rnd = new Random();
            int index1 = rnd.Next(0, FacultiesNames.Count);
            int index2 = rnd.Next(0, FacultiesNames.Count);
            while (index2 == index1)
            {
                index2 = rnd.Next(0, FacultiesNames.Count);
            }
            List<string> result = new List<string>{
                SubjectAdmissions[index1],
                SubjectAdmissions[index2]
                };
            return result;
        }
        public int MinRatingAdmission()
        {
            Random rnd = new Random();
            return rnd.Next(10, 40);
        }
        public Faculty GetFaculty(string FacultyName)
        {
            foreach (var item in faculties)
            {
                if (item.Name == FacultyName)
                {
                    return item;
                }
            }
            throw new Exception("invalid Faculty");
        }
        public University(string Name, DateTime dateTime)
        {
            int index = 0;
            foreach (var item in FacultiesNames)
            {
                faculties.Add(new Faculty(item, new Admission(MinRatingAdmission(), GetSubjectsAdmissions()), GetSubjectsOfFaculty(index), dateTime));
                if (faculties[index].Students != null)
                {
                    if (faculties[index].Students.Count != 0)
                    {
                        faculties[index].DateTime = faculties[index].DateTime.AddMonths(6);
                        if (DateTime == faculties[index].DateTime)
                        {
                            faculties[index].CreateExam(Students, GetSubjectsOfFaculty(index));
                        }
                    }
                }
                index++;
            }
            this.Name = Name;
            this.DateTime = dateTime;
        }
        public void AddStudent(Person person)
        {
            string FacultyName = person.FacultyName;
            string PassportId = person.PassportId;
            int IdNumber = person.IdNumber;
            string FirstName = person.FirstName;
            string LastName = person.LastName;
            Dictionary<string, int> SubjectsAndRatings = person.SubjectsAndRatings;
            if (IdNumber == 0 && PassportId == null)
            {
                throw new Exception("invalid IdNumber and PassportId");
            }
            int Count = 0;
            foreach (string item in FacultiesNames)
            {
                if (item == FacultyName)
                {
                    int SumOfRatings = 0;
                    foreach (var Ratings in SubjectsAndRatings)
                    {
                        SumOfRatings += Ratings.Value;
                    }
                    if (SumOfRatings < GetFaculty(FacultyName).admission.MinRating)
                    {
                        throw new Exception("Rating is Lower then minRating");
                    }
                    break;
                }
                else
                {
                    Count++;
                    if (Count == FacultiesNames.Count)
                    {
                        throw new Exception("invalid Faculty");
                    }
                }
            }
            Students.Add(new Student(FirstName, LastName, StudentsCount + 1, FacultyName));
            foreach (var faculty in faculties)
            {
                if (faculty.Name == FacultyName)
                {
                    faculty.Students.Add(new Student(FirstName, LastName, StudentsCount + 1, FacultyName));
                    break;
                }
            }
            StudentsCount++;
            if (StudentsCount == MaxStudentCount)
            {
                MaxStudentCount *= 2;
            }
        }

    }

    public class Faculty
    {
        public DateTime DateTime;
        public string Name;
        public List<Student> Students = new List<Student>();
        public Admission admission;
        public List<string> SubjectsOfFaculty;

        public Faculty(string Name, Admission admission, List<string> SubjectsOfFaculty, DateTime dateTime)
        {
            this.DateTime = dateTime;
            this.Name = Name;
            this.admission = admission;
            this.SubjectsOfFaculty = SubjectsOfFaculty;
            DateTime = DateTime.Now;
        }
        public void CreateExam(List<Student> Students, List<string> SubjectsOfFaculty)
        {
            new Exam(SubjectsOfFaculty, Students);
        }
    }

    public class Student
    {
        public DateTime AdmissionTime;
        public string FirstName;
        public string LastName;
        public int Id;
        public string FacultyName;
        public Dictionary<string, int> SubjectsAndRatings;
        public Student(string FirstName, string LastName, int Id, string FacultyName)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Id = Id;
            this.FacultyName = FacultyName;
            AdmissionTime = DateTime.Now;
        }
    }
    public class Person
    {
        public string FirstName;
        public string LastName;
        public string PassportId;
        public int IdNumber;
        public Dictionary<string, int> SubjectsAndRatings;
        public string FacultyName;
        public Person(string firstName, string lastName, string passportId, int idNumber, Dictionary<string, int> subjectsAndRatings, string FacultyName)
        {
            FirstName = firstName;
            LastName = lastName;
            PassportId = passportId;
            IdNumber = idNumber;
            SubjectsAndRatings = subjectsAndRatings;
            this.FacultyName = FacultyName;
        }
    }
    //himnakan
    public class Admission
    {
        public List<string> SubjectsNames;
        public int MinRating;
        public Admission(int MinRating, List<string> SubjectsNames)
        {
            this.MinRating = MinRating;
            this.SubjectsNames = SubjectsNames;
        }
    }
    public class Exam
    {
        public List<string> SubjectsName;
        public List<Student> students;
        public Exam(List<string> SubjectsName, List<Student> students)
        {
            this.SubjectsName = SubjectsName;
            this.students = students;
            foreach (var Subject in SubjectsName)
            {
                foreach (var Student in students)
                {
                    new RatingsOfStudents(Student, Subject);
                }
            }
        }
    }
    public class RatingsOfStudents
    {
        public Student Student;
        public string Subject;
        public int RatingsOfSubject;
        public RatingsOfStudents(Student student, string Subject)
        {
            this.Student = student;
            this.Subject = Subject;
            Random random = new Random();
            student.SubjectsAndRatings.Add(Subject, random.Next(1, 20));
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string UniversityName = "Politech";
            DateTime dateTime = DateTime.Now;
            University university = new University(UniversityName, dateTime);


            var subjectsAndRatings2 = new Dictionary<string, int>
            {
                { "Computer Science", 25 },
                { "English", 20 }
            };
            var person2 = new Person("bob", "johnsn", "325431", 2, subjectsAndRatings2, "Mathematics");
            try
            {
                university.AddStudent(person2);
                Console.WriteLine($"Student {person2.FirstName} {person2.LastName} added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add student: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
