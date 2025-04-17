using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CantinaAPI.Infrastructure.Data;

var services = new ServiceCollection();

services.AddDbContext<CantinaContext>(options =>
    options.UseSqlite("Data Source=DB/Cantina.db"));

var provider = services.BuildServiceProvider();
var context = provider.GetRequiredService<CantinaContext>();

context.Database.Migrate(); // work around for assembly migration issues 