using IserviceColections_MapWhen.DBContextLayer;
using IserviceColections_MapWhen.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IserviceColections_MapWhen.Sevices
{
   
    public class AppSeviceStudents
    {
        public readonly AppDBContext _appDBContext;

        public AppSeviceStudents(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public async Task<List<Students>> ReadStudents()
        {
            var listStudents = await _appDBContext.students.ToListAsync();
            return listStudents;
        }

        public async Task DeleteStudent(Guid Id)
        {
            try
            {
                var find = await (from a in _appDBContext.students
                                  where (a.Id == Id)
                                  select a).FirstOrDefaultAsync();
                if (find != null)
                {
                    _appDBContext.Remove(find);

                    await _appDBContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Xao khong thanh cong" + e);
            }
            finally
            {
                Console.WriteLine("Xoa thanh cong");
            }
        }

        public async Task InsertStudent(Students students)
        {
            try
            {

                await _appDBContext.students.AddAsync(students);

                await _appDBContext.SaveChangesAsync();
            }
            catch (Exception e)
            {

                Console.WriteLine("loi " + e);

            }
            finally
            {
                Console.WriteLine("Luu thanh cong");
            }


        }

        public async Task Rename(Students students)
        {
            try
            {               
                    var existingGrader = await (from p in _appDBContext.students where (p.Id == students.Id) select p).FirstOrDefaultAsync();

                    if (existingGrader != null)
                    {
                        existingGrader.Name = students.Name;
                        existingGrader.Age = students.Age;

                        await _appDBContext.SaveChangesAsync();
                    }
            }
            catch (Exception e)
            {

                Console.WriteLine("loi khong thanh cong " + e);
            }
            finally
            {
                Console.WriteLine("Sua Thanh Cong");
            }
        }

        public async Task<Students> GetStudents(Guid Id)
        {
          
            try
            {
                var students = await (from p in _appDBContext.students where (p.Id == Id) select p).FirstOrDefaultAsync();

                return students;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Console.WriteLine("get thanh cong");
               
            }

           
        }
    }
}
