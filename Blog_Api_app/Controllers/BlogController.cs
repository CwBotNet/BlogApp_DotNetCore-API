using Microsoft.AspNetCore.Mvc;
using Blog_Api_app.Data;
using Microsoft.EntityFrameworkCore;
using Blog_Api_app.Models;
using Microsoft.Extensions.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Blog_Api_app.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BlogController : ControllerBase
    {


        public readonly MyDbContext _context; // Dependency injection for database context

        public BlogController(MyDbContext context) // Constructor to inject context
        {
            _context = context;
        }


        // POST function (create a new blog post)


        [HttpPost]
        public async Task<IActionResult> PostBlog([FromBody] AddBlog data)
        {
            try
            {

                if (data == null) // Check for valid data
                {
                    return NoContent(); // Return empty response if no data
                }
                else
                {
                    // Create a new blog post object from the provided data
                    BlogPost _data = new BlogPost
                    {
                        PageTitle = data.PageTitle,
                        Content = data.Content,
                        ShortDescription = data.ShortDescription,
                        FeaturedImageUrl = data.FeaturedImageUrl,
                        UrlHandle = data.UrlHandle,
                        Author = data.Author,
                        Visible = data.Visible

                    };

                    // Add the blog post to the database and save changes
                    _context.BlogPosts.Add(_data);
                    await _context.SaveChangesAsync();
                    return new JsonResult(Ok("Blog added successfully")); // Return success message
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return error message on failure
            }
        }

        // GET functions (retrieve blog posts)

        [HttpGet]

        public JsonResult GetAll() // Get all blog posts
        {
            var blogs = _context.BlogPosts.ToList(); // Fetch all from database

            return new JsonResult(blogs); // Return as JSON
        }

        // Get a specific blog post by ID

        [HttpGet("{id}")]
        public JsonResult GetBlog(Guid id) 
        {
            try
            {
                var blog = _context.BlogPosts.FirstOrDefault(x => x.Id == id); // Find by ID
                if (blog == null)
                {
                    return new JsonResult(NotFound()); // Return 404 if not found
                }

                return new JsonResult(blog); // Return the blog post data
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message); // Return error message
            }
        }




        //// PUT function (update an existing blog post)

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(Guid id, [FromBody] AddBlog data) // Bind data from request body
        {
            try
            {
                var blogById = _context.BlogPosts.Find(id); // Fetch existing blog post
                if (blogById == null)
                {
                    return new JsonResult(NotFound()); // Return 404 if not found
                }

                // Update properties of the existing blog post
                blogById.PageTitle = data.PageTitle;
                blogById.Content = data.Content;
                blogById.ShortDescription = data.ShortDescription;
                blogById.FeaturedImageUrl = data.FeaturedImageUrl;
                blogById.UrlHandle = data.UrlHandle;
                blogById.Author = data.Author;
                blogById.Visible = data.Visible;

                _context.SaveChanges(); // Save changes to database

                return new JsonResult(Ok(blogById)); // Return updated blog post
            } catch (Exception ex)
            {
                return new JsonResult(ex); // Return error message
            }
        }

        // DELETE function (delete a blog post)

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id) 
        {
            try
            {
                var blog = await _context.BlogPosts.FindAsync(id); // Find the blog post
                if (blog == null)
                {
                    return NotFound(); // Return 404 if not found
                }
                else
                {
                    _context.BlogPosts.Remove(blog); // Remove from database
                    _context.SaveChanges(); // Save changes
                }

                return new JsonResult(Ok(new {blog} + " " + "is deleted successfully")); // Return success message    
            }
            catch(Exception ex)
            {
                return new JsonResult(ex); // Return error message
            }
        }
        
    }
}
