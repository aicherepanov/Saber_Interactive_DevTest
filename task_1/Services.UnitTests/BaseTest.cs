using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services.UnitTests;

public abstract class BaseTest
{
    protected IServiceProvider? ServiceProvider { get; private set; }
    
    [TestInitialize]
    public virtual void Initialize()
    {
        var services = new ServiceCollection();
        services.RegisterServicesDependencies();
        ServiceProvider = services.BuildServiceProvider();
    }
    
    [TestCleanup]
    public virtual void Cleanup()
    {
        ServiceProvider = null;
    }
}