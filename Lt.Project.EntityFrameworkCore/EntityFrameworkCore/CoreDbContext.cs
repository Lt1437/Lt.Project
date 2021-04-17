using Lt.Project.Domain.TestCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lt.Project.EntityFrameworkCore.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文（建立实体类与数据库连接）
    /// </summary>
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 测试实体
        /// </summary>
        public virtual DbSet<Test> Test { get; set; }//创建测试实体类添加Context中
    }
}