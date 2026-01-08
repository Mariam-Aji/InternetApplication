using System.ComponentModel.DataAnnotations;

namespace WebAPI.Domain.Entities
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }     // هوية المستخدم (المعرف)
        public string UserName { get; set; }   // هوية المستخدم (الاسم الكامل)
        public string Action { get; set; }     // العملية المنفذة
        public string Controller { get; set; } // المتحكم (سياق العملية)
        public DateTime Timestamp { get; set; } // التاريخ والوقت
        public long ExecutionTimeMs { get; set; } // سرعة التفاعل (المتطلب رقم 7)

        // حقل إضافي لتجنب خطأ SQL (نرسل قيمة افتراضية)
        public string IpAddress { get; set; } = "Internal";
    }
}