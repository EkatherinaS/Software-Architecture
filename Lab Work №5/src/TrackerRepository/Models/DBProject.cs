using System.ComponentModel.DataAnnotations;

namespace TrackerRepository
{
    public class DBProject : DBEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public bool Equals(DBProject obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (this.Name == obj.Name) &&
                ((this.Description is null && obj.Description is null) || 
                (this.Description != null && obj.Description != null && this.Description == obj.Description));
        }

        public ProjectEntity toProjectEntity()
        {
            ProjectEntity projectEntity = new ProjectEntity();
            projectEntity.ProjectName = this.Name;
            projectEntity.ProjectDescription = this.Description;
            return projectEntity;
        }
    }
}
