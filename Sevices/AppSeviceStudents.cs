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
    }
}
