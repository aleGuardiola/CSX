
using Android.Graphics;
using System.Reflection;

namespace CSX.Android.Test
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource


            //var root = new FrameLayout(this);
            //root.SetBackgroundColor(Color.Red);

            //SetContentView(Resource.Layout.activity_main);
            var dom = new AndroidDom(this);

            CSXHostBuilder.Create(new string[0], dom)
                .ConfigureServices((context, services) =>
                {
                    services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
                })
                .Build()
                .StartAsync<ComponentTest, TestProps>(new() { Name = "Alejandro", LastName = "Guardiola" });

            //var view = new RelativeLayout(this);
            //view.SetBackgroundColor(Color.Red);
            //SetContentView(view);
            //SetContentView(Resource.Layout.activity_main);

        }

       

    }
}