namespace Nancy.Authentication.OAuth
{
    using System;

    /// <summary>
    /// Responsible for creating the model that should be passed to the view that gives the
    /// client the alternative to Allow or Deny access to the account.
    /// </summary>
    public interface IViewModelDecorator
    {
        dynamic CreateViewModel(dynamic model);
    }

    public class DefaultViewModelDecorator : IViewModelDecorator
    {
        public dynamic CreateViewModel(dynamic model)
        {
            model.GeneratedAt = DateTime.Now.ToString();

            return model;
        }
    }
}