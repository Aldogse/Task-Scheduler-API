using System.Linq.Expressions;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using TaskScheduler.Contracts;
using TaskSchedulerAPI.Data;
using TaskSchedulerAPI.Interfaces;
using TaskSchedulerAPI.Model;


namespace TaskSchedulerAPI.Controller
{
    [ApiController]
    [Authorize]
    [Route("/Task/v1")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ApplicationDBContext _context;

        public TaskController(ITaskRepository taskRepository , ApplicationDBContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }

        [HttpGet("all")]
        [Authorize]
        public IActionResult GetTasks()
        {
            var request = _taskRepository.GetAllTasks();
            var response = new List<TaskResponse>();

            if (request == null)
            {
               return StatusCode(204,"No available Task yet.");
            }
        
            foreach (var item in request)
            {
                string owner_name = $"{item.Owner.FirstName} {item.Owner.LastName}";
                var task = new TaskResponse(item.Id,
                                            item.TaskName,
                                            item.Description,
                                            item.StartDateAndTime.ToString(),
                                            item.EndDateAndTime.ToString(),
                                            owner_name,
                                            item.Status.Name,
                                            item.ExpirationDate.ToString(),
                                            item.TaskLevel.Level);

                response.Add(task);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var user = await _taskRepository.GetTaskByIdAsync(id);
            if(user == null)
            {
                return StatusCode(404,"No user found");
            }
            string owner_name = $"{user.Owner.FirstName} {user.Owner.LastName}";

            var response = new TaskResponse(
                user.Id,
                user.TaskName,
                user.Description,
                user.StartDateAndTime.ToString(),
                user.EndDateAndTime.ToString(),
                owner_name,
                user.Status.Name ?? "Unknown",
                user.ExpirationDate.ToString()        ,
                user.TaskLevel.Level
                );
            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize]       
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newTask.TaskLevel_id > 3 || newTask.status_id > 3)
            {
                 return BadRequest();
            }
            DateTime TaskStartDate = DateTime.Parse($"{newTask.StartDate} {newTask.StartTime}");
            DateTime TaskEndDate = DateTime.Parse($"{newTask.EndDate} {newTask.EndTime}");

            var request = new TaskModel(
                newTask.TaskName,
                newTask.Description,
                TaskStartDate,
                TaskEndDate,
                newTask.Owner_id,
                newTask.status_id,
                newTask.ExpirationDate,
                newTask.TaskLevel_id,
                DateTime.Now
                );


              await _context.AddAsync(request);
             await _context.SaveChangesAsync();

            var taskWithRelation = await _context.Tasks.Where(t => t.Id == request.Id)
                .Include(t => t.Status)
                .Include(t => t.TaskLevel).FirstOrDefaultAsync();

            if( taskWithRelation == null ) 
            {
                return NotFound();
            }

            string owner_name = $"{taskWithRelation.Owner.FirstName} {taskWithRelation.Owner.LastName}";
            var response = new TaskResponse(
                taskWithRelation.Id,
                taskWithRelation.TaskName,
                taskWithRelation.Description,
                taskWithRelation.StartDateAndTime.ToString(),
                taskWithRelation.EndDateAndTime.ToString(),
                owner_name,
                taskWithRelation.Status.Name,
                taskWithRelation.ExpirationDate.ToString(),
                taskWithRelation.TaskLevel.Level

                );

            return CreatedAtAction(
                nameof(GetTasks),
                new {id = request.Id},
                response
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest updateRequest, [FromRoute] int id)
        {
            var taskToUpdate = await _context.Tasks.FindAsync(id);

            //check if the id to update Exist
            if(taskToUpdate == null)
            {
                return StatusCode(404,"No task associated with the id you provided.");
            }
            //check if the value passed are correct
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            taskToUpdate.Update(
                updateRequest.TaskName,
                updateRequest.Description,
                updateRequest.StartDateAndTime,
                updateRequest.EndDateAndTime,
                updateRequest.Owner ?? "Unknown",
                updateRequest.status_id,
                updateRequest.ExpirationDate,
                updateRequest.TaskLevel_id,
                DateTime.Now
                );
            _context.Tasks.Update(taskToUpdate);
            await _context.SaveChangesAsync();
            string owner_name = $"{taskToUpdate.Owner.FirstName} {taskToUpdate.Owner.LastName}";
            var response = new TaskResponse(
                taskToUpdate.Id,
                taskToUpdate.TaskName,
                taskToUpdate.Description,
                taskToUpdate.StartDateAndTime.ToString(),
                taskToUpdate.EndDateAndTime.ToString(),
                owner_name,
                taskToUpdate.Status.Name,
                taskToUpdate.ExpirationDate.ToString(),
                taskToUpdate.TaskLevel.Level

                );

            return Ok(response);
        }

        [HttpGet("available")]
        [Authorize(Roles ="User")]
        public IActionResult GetAvailableTask()
        {
            try
            {
                var noOwners = _taskRepository.GetTasksWithNoOwner();

                if (!noOwners.Any() || noOwners.Count == 0)
                {
                    return Ok(new {
                        message = "All Task has an owner",
                        tasks_with_no_owner = new List<StatusModel>()
                    });
                }

                var available = new List<TaskResponse>();

                foreach (var task in noOwners)
                {
                    string owner_name = $"{task.Owner.FirstName} {task.Owner.LastName}";
                    var response = new TaskResponse(
                                   task.Id,
                                   task.TaskName,
                                   task.Description,
                                   task.StartDateAndTime.ToString(),
                                   task.EndDateAndTime.ToString(),
                                   owner_name,
                                   task.Status.Name,
                                   task.ExpirationDate.ToString(),
                                   task.TaskLevel.Level
                                  );
                    available.Add(response);
                }
                return Ok(available);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error : {ex.Message}");
            }
        }

        [HttpGet("TaskLevel/{id}")]
        public IActionResult GetTaskByPriority([FromRoute]int id)
        {

            if(id == 0 || id > 3)
            {
                return StatusCode(400,new
                {
                    message = "Number should not equal to Zero or exceed to 3"
                });
            }
            var tasks = _taskRepository.GetTaskByUrgency(id);
         
            var tasks_list = new List<TaskResponse>();
            var task_level_name = _taskRepository.GetTaskLevel(id);
            if (tasks.Count == 0 || !tasks.Any())
            {
                return Ok(new
                {
                    message = $"No {task_level_name.Level} level task as of the moment.",
                });
            }

            foreach (var item in tasks)
            {

                string owner_name = $"{item.Owner.FirstName} {item.Owner.LastName}";
                var response = new TaskResponse(
                    item.Id,
                    item.TaskName,
                    item.Description,
                    item.StartDateAndTime.ToString(),
                    item.EndDateAndTime.ToString(),
                    owner_name,
                    item.Status.Name,
                    item.ExpirationDate.ToString(),
                    item.TaskLevel.Level
                    );
                tasks_list.Add(response);
            }

            return Ok(tasks_list);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] int id)
        {
            //find the task
            var user = _context.Tasks.Find(id);

            _context.Remove(user);
            _context.SaveChanges();
            return Ok(ModelState);
        }

        [HttpGet("UserTask/{id}")]
        public IActionResult GetUsersTask([FromRoute] string id)
        {
            try
            {
                var tasks_of_user = _taskRepository.GetUsersTasks(id);
                if (tasks_of_user == null)
                {
                    return NoContent();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tasks = new List<TaskResponse>();
                foreach (var task in tasks_of_user)
                {
                    string owner_name = $"{task.Owner.FirstName} {task.Owner.LastName}";
                    var response = new TaskResponse(
                        task.Id,
                        task.TaskName,
                        task.Description,
                        task.StartDateAndTime.ToString(),
                        task.EndDateAndTime.ToString(),
                        owner_name,
                        task.Status.Name,
                        task.ExpirationDate.ToString(),
                        task.TaskLevel.Level
                        );
                    tasks.Add(response);
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("PaginatedRequest")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PaginatedRequest([FromQuery]int page_size, [FromQuery]int page_count)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var paginated_request = await _taskRepository.PaginatedRequest(page_size, page_count);
                return Ok(paginated_request);
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Internal server error {ex.Message}");
            }
        }
    }
}
 