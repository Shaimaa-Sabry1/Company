﻿using System.ComponentModel.DataAnnotations;

namespace CompanyMVC.PL.Dtos
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage ="Code Is Required !")]
        public string Code { get; set; }
        
        [Required(ErrorMessage = "Name Is Required !")]
        public string Name { get; set; }
        [Required(ErrorMessage = "CraeteAt Is Required !")]
        public DateTime CreateAt { get; set; }
    }
}
