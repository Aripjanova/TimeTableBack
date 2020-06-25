using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using WebApplication;
using WebApplication.Controllers;
using WebApplication.Models;

namespace  WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {



            int[] groups = new int[]
            {
                5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
                55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55,
                66, 66, 66, 66, 66, 66, 66, 66, 66, 66, 66, 66, 66, 66,
                6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
                77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77, 77,
                7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
                88, 88, 88, 88, 88, 88, 88, 88, 88, 88, 88, 88, 88, 88, 88,
                8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
                99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
                9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            };
            int[] teachers = new int[]
            {
                12, 25, 8, 22, 3, 17, 27, 222, 15, 20, 44, 2222, 18, 28,
                13, 25, 8, 22, 3, 17, 27, 222, 15, 20, 1212, 2222, 18, 28, 9, 27, 26, 11, 4, 21, 22, 222, 24, 23, 21,
                111, 19, 28,
                9, 27, 26, 11, 4, 21, 22, 222, 24, 23, 21, 1414, 19, 28,
                12, 10, 26, 8, 4, 29, 2929, 24, 27, 15, 20, 1717, 292, 18, 28, 11, 1212,
                12, 10, 26, 8, 4, 29, 2929, 24, 27, 15, 20, 2020, 292, 18, 28, 11, 1212,
                13, 1, 22, 222, 3, 25, 9, 21, 17, 15, 13, 1313, 19, 10, 99,
                13, 1, 22, 222, 3, 25, 9, 21, 17, 15, 13, 1515, 19, 10, 99,
                12, 26, 8, 29, 3, 24, 19, 15, 20, 55, 29, 1, 10, 8, 12,
                12, 26, 8, 29, 3, 24, 19, 15, 20, 1616, 29, 1, 10, 8, 12
            };

            var list = new List<Lessоn>();
            for (int i = 0; i < groups.Length; i++)
                list.Add(new Lessоn(groups[i], teachers[i]));

            var solver = new Solver(); 

            Plan.DaysPerWeek =5; 
            Plan.HoursPerDay = 6;

            solver.FitnessFunctions.Add(FitnessFunctions.Windows); 
            solver.FitnessFunctions.Add(FitnessFunctions.LateLesson); 

            var res = solver.Solve(list); 

            Console.WriteLine(res);
            
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        
        static class FitnessFunctions
        {
            public static int GroupWindowPenalty = 10; 
            public static int TeacherWindowPenalty = 7;
            public static int LateLessonPenalty = 1; 

            public static int LatesetHour = 3; 

        
            public static int Windows(Plan plan)
            {
                var res = 0;

                for (byte day = 0; day < Plan.DaysPerWeek; day++)
                {
                    var groupHasLessions = new HashSet<int>();
                    var teacherHasLessions = new HashSet<int>();

                    for (byte hour = 0; hour < Plan.HoursPerDay; hour++)
                    {
                        foreach (var pair in plan.HourPlans[day, hour].GroupToTeacher)
                        {
                            var group = pair.Key;
                            var teacher = pair.Value;
                            if (groupHasLessions.Contains(group) &&
                                !plan.HourPlans[day, hour - 1].GroupToTeacher.ContainsKey(group))
                                res += GroupWindowPenalty;
                            if (teacherHasLessions.Contains(teacher) &&
                                !plan.HourPlans[day, hour - 1].TeacherToGroup.ContainsKey(teacher))
                                res += TeacherWindowPenalty;

                            groupHasLessions.Add(group);
                            teacherHasLessions.Add(teacher);
                        }
                    }
                }

                return res;
            }

        
            public static int LateLesson(Plan plan)
            {
                var res = 0;
                foreach (var pair in plan.GetLessons())
                    if (pair.Hour > LatesetHour)
                        res += LateLessonPenalty;

                return res;
            }
        }

        
        ///  (генетикалык алгоритм)
        
        class Solver
        {
            public int MaxIterations = 1000;
            public int PopulationCount = 100; 

            public List<Func<Plan, int>> FitnessFunctions = new List<Func<Plan, int>>();

            public int Fitness(Plan plan)
            {
                var res = 0;

                foreach (var f in FitnessFunctions)
                    res += f(plan);

                return res;
            }

            public Plan Solve(List<Lessоn> pairs)
            {
                //создаем популяцию
                var pop = new Population(pairs, PopulationCount);
                if (pop.Count == 0)
                    throw new Exception("Can not create any plan");
               
                var count = MaxIterations;
                while (count-- > 0)
                {
                   
                    pop.ForEach(p => p.FitnessValue = Fitness(p));
                   
                    pop.Sort((p1, p2) => p1.FitnessValue.CompareTo(p2.FitnessValue));
                  
                    if (pop[0].FitnessValue == 0)
                        return pop[0];
                  
                    pop.RemoveRange(pop.Count / 4, pop.Count - pop.Count / 4);
                   
                    var c = pop.Count;
                    for (int i = 0; i < c; i++)
                    {
                        pop.AddChildOfParent(pop[i]);
                        pop.AddChildOfParent(pop[i]);
                        pop.AddChildOfParent(pop[i]);
                    }
                }

               
                pop.ForEach(p => p.FitnessValue = Fitness(p));
              
                pop.Sort((p1, p2) => p1.FitnessValue.CompareTo(p2.FitnessValue));

               
                return pop[0];
            }
        }

      
        /// пландын популяциясы
      
        class Population : List<Plan>
        {
            public Population(List<Lessоn> pairs, int count)
            {
                var maxIterations = count * 2;

                do
                {
                    var plan = new Plan();
                    if (plan.Init(pairs))
                        Add(plan);
                } while (maxIterations-- > 0 && Count < count);
            }

            public bool AddChildOfParent(Plan parent)
            {
                int maxIterations = 10;

                do
                {
                    var plan = new Plan();
                    if (plan.Init(parent))
                    {
                        Add(plan);
                        return true;
                    }
                } while (maxIterations-- > 0);

                return false;
            }
        }

      
        class Plan
        {
            public static int DaysPerWeek = 5; 
            public static int HoursPerDay = 6;

            static Random rnd = new Random(3);

          
            public HourPlan[,] HourPlans = new HourPlan[DaysPerWeek, HoursPerDay];

            public int FitnessValue { get; internal set; }

            public bool AddLesson(Lessоn les)
            {
                return HourPlans[les.Day, les.Hour].AddLesson(les.Group, les.Teacher);
            }

            public void RemoveLesson(Lessоn les)
            {
                HourPlans[les.Day, les.Hour].RemoveLesson(les.Group, les.Teacher);
            }

          
            public bool AddToAnyDayAndHour(int group, int teacher)
            {

                int maxIterations = 30;
                do
                {
                    var day = (byte) rnd.Next(DaysPerWeek);
                    if (AddToAnyHour(day, group, teacher))
                        return true;
                } while (maxIterations-- > 0);

                return false; 
            }

         
            bool AddToAnyHour(byte day, int group, int teacher)
            {
                for (byte hour = 0; hour < HoursPerDay; hour++)
                {
                    var les = new Lessоn(day, hour, group, teacher);
                    if (AddLesson(les))
                        return true;
                }

                return false; //бул куну башка бош саат жоок
            }

            
            /// сабактардын тизмеси боюнча план тузуу
           
            public bool Init(List<Lessоn> pairs)
            {
                for (int i = 0; i < HoursPerDay; i++)
                for (int j = 0; j < DaysPerWeek; j++)
                    HourPlans[j, i] = new HourPlan();

                foreach (var p in pairs)
                    if (!AddToAnyDayAndHour(p.Group, p.Teacher))
                        return false;
                return true;
            }

           
            
           /// мутациейдан наследникати тузуу
           
            public bool Init(Plan parent)
            {
              
                for (int i = 0; i < HoursPerDay; i++)
                for (int j = 0; j < DaysPerWeek; j++)
                    HourPlans[j, i] = parent.HourPlans[j, i].Clone();

               
                var day1 = (byte) rnd.Next(DaysPerWeek);
                var day2 = (byte) rnd.Next(DaysPerWeek);

                
                var pairs1 = GetLessonsOfDay(day1).ToList();
                var pairs2 = GetLessonsOfDay(day2).ToList();

                if (pairs1.Count == 0 || pairs2.Count == 0) return false;
                var pair1 = pairs1[rnd.Next(pairs1.Count)];
                var pair2 = pairs2[rnd.Next(pairs2.Count)];
                
                RemoveLesson(pair1);
                RemoveLesson(pair2);
                var res1 = AddToAnyHour(pair2.Day, pair1.Group, pair1.Teacher); 
                var res2 = AddToAnyHour(pair1.Day, pair2.Group, pair2.Teacher); 
                return res1 && res2;
            }

            public IEnumerable<Lessоn> GetLessonsOfDay(byte day)
            {
                for (byte hour = 0; hour < HoursPerDay; hour++)
                    foreach (var p in HourPlans[day, hour].GroupToTeacher)
                        yield return new Lessоn(day, hour, p.Key, p.Value);
            }

            public IEnumerable<Lessоn> GetLessons()
            {
                for (byte day = 0; day < DaysPerWeek; day++)
                for (byte hour = 0; hour < HoursPerDay; hour++)
                    foreach (var p in HourPlans[day, hour].GroupToTeacher)
                    {
                       // var a = new SetTableController(day, hour, p.Key, p.Value);
                        yield return new Lessоn(day, hour, p.Key, p.Value);
                        
                    }
                        
            }

            public override string ToString()
            { 
                int[,] Group = new int[1000,1000];
                int[,] Teah = new int[1000,1000];
                var sb = new StringBuilder();
              
                for (byte day = 0; day < Plan.DaysPerWeek; day++)
                {
                    sb.AppendFormat("Day {0}\r\n", day);
                    for (byte hour = 0; hour < Plan.HoursPerDay; hour++)
                    {
                        
                       
                        sb.AppendFormat("Hour {0}: ", hour);
                        foreach (var p in HourPlans[day, hour].GroupToTeacher)
                        {
                            MySqlConnection conDatabase = new MySqlConnection("Server=localhost;Database=timetable;Uid=root;Pwd=aika9709;");
                            string cmd_cnt;

                            conDatabase.Open();
                            //   cmd_cnt = "select first_name from teacher";
                            cmd_cnt = " INSERT INTO `schedule` (`day`, `hour`,`group`,`teacher`) VALUES ("+day+","+hour+","+p.Key+","+p.Value+")";
                
                            string strName;

                            MySqlCommand db_cmd = new MySqlCommand(cmd_cnt, conDatabase);
                            MySqlDataReader dbread = db_cmd.ExecuteReader();

                            if (dbread.Read())
                            {
                                strName= dbread[0].ToString();
                                sb.AppendFormat("Aizaaaad {0} ",strName.ToString());

                            }
                
                            conDatabase.Close();
                         var a = new GetData(day,hour,p.Key,p.Value);
                         //   a.AddBlock(day, hour, p.Key, p.Value);
                            Group[day, hour] = p.Key;
                                Teah[day, hour] = p.Value;
                                if(p.Value==15)
                                sb.AppendFormat("Tch: {0}-{1} ", Group[day, hour],  Teah[day, hour]);
                                
                            
                        }
                      
                        sb.AppendLine();
                    }
                }
               
                
                int[,] Groupp = new int[1000,1000];
                int[,] Teahh = new int[1000,1000];
                Groupp = Group;
                Teahh = Teah;
              
                for (int i = 0; i < 6; i++)
                {
                    sb.AppendFormat("Day {0}\r\n", i);
                    for (int j = 0; j < 6; j++)
                    {   sb.AppendFormat("Hour {0}: ", j);
                        sb.AppendFormat("Gr-Tch: {0}-{1} ", Groupp[i, j],  Teahh[i, j]);
                    }
                   
                }

                sb.AppendFormat("Fitness: {0}\r\n", FitnessValue);

                return sb.ToString();
            }
        }

        
        /// саатка карата пландоо
        
        class HourPlan
        {
           
           
          
            public Dictionary<int, int> GroupToTeacher = new Dictionary<int, int>();

          
          
            
            public Dictionary<int, int> TeacherToGroup = new Dictionary<int, int>();

            public bool AddLesson(int group, int teacher)
            {
                if (TeacherToGroup.ContainsKey(teacher) || GroupToTeacher.ContainsKey(group))
                    return false; 

                GroupToTeacher[group] = teacher;
                TeacherToGroup[teacher] = group;

                return true;
            }

            public void RemoveLesson(int group, int teacher)
            {
                GroupToTeacher.Remove(group);
                TeacherToGroup.Remove(teacher);
            }

            public HourPlan Clone()
            {
                var res = new HourPlan();
                res.GroupToTeacher = new Dictionary<int, int>(GroupToTeacher);
                res.TeacherToGroup = new Dictionary<int, int>(TeacherToGroup);

                return res;
            }
        }

      
     
     
      
    }

    public class Lessоn
    {
        public byte Day = 255;
        public byte Hour = 255;
        public int Group;
        public int Teacher;

        public Lessоn(byte day, byte hour, int group, int teacher)
            : this(group, teacher)
        {
            Day = day;
            Hour = hour;
        }

        public Lessоn(int group, int teacher)
        {
            Group = group;
            Teacher = teacher;
        }
    }
}

