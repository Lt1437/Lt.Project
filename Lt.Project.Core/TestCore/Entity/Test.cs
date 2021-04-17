using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lt.Project.Domain.TestCore.Entity
{
    /// <summary>
    /// 测试实体
    /// </summary>
    public class Test: FullAuditedEntity<int>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}
