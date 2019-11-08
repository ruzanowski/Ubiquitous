namespace Caracan.Templates.Manager
{
    public interface IFluidBinder
    {
        /// <summary>
        /// Returns bound templateObject for given name with given liquid object notificationNavBarToggled
        /// </summary>
        /// <param name="fluidTemplate"></param>
        /// <param name="liquidTemplateObjectData"></param>
        /// <returns></returns>
        string Bind(string fluidTemplate, object liquidTemplateObjectData);

    }
}