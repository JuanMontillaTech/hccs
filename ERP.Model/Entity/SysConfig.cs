namespace ERP.Domain.Entity
{
    public class SysConfig : Audit
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
