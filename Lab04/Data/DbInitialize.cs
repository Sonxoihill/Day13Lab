using Lab04.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab04.Data
{
    public class DbInitialize
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SchoolDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<SchoolDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Major.Any())
                {
                    return;
                }

                var majors = new Major[]
                {
            new Major { MajorName = "IT" },
            new Major { MajorName = "Economics" },
            new Major { MajorName = "Mathematics" }
                };

                foreach (var major in majors)
                {
                    context.Major.Add(major);
                }
                context.SaveChanges();

                var learners = new Learner[]
                {
            new Learner
            {
                FirstMidName = "Carson",
                LastName = "Alexander",
                EnrollmentDate = DateTime.Parse("2005-09-01"),
                MajorId = 1
            },
            new Learner
            {
                FirstMidName = "Meredith",
                LastName = "Alonso",
                EnrollmentDate = DateTime.Parse("2002-09-01"),
                MajorId = 2
            }
                };

                foreach (Learner l in learners)
                {
                    context.Learner.Add(l);
                }
                context.SaveChanges();

                var courses = new Course[]
                {
            new Course { CourseId = 1050, Title = "Chemistry", Credits = 3 },
            new Course { CourseId = 4022, Title = "Microeconomics", Credits = 3 },
            new Course { CourseId = 4041, Title = "Macroeconomics", Credits = 3 }
                };

                foreach (Course c in courses)
                {
                    context.Course.Add(c);
                }
                context.SaveChanges();

                var enrollments = new Enrollment[]
                {
            new Enrollment { LearnerId = 1, CourseId = 1050, Grade = 5.5f },
            new Enrollment { LearnerId = 1, CourseId = 4022, Grade = 7.5f },
            new Enrollment { LearnerId = 2, CourseId = 1050, Grade = 3.5f },
            new Enrollment { LearnerId = 2, CourseId = 4041, Grade = 7f }
                };

                foreach (Enrollment e in enrollments)
                {
                    context.Enrollment.Add(e);
                }

                context.SaveChanges();
            }
        }

    }
}
