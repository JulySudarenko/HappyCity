using Code.Configs;
using Code.View;


namespace Code.ViewHandlers
{
    internal class ResourceCounterViewHandler
    {
        private readonly ImageLineElement _resElement;

        public ResourceCounterViewHandler(ImageLineElement resElement, ResourcesConfig config)
        {
            _resElement = resElement;
            _resElement.gameObject.SetActive(true);
            _resElement.Icon.sprite = config.Icon;
            _resElement.Description.text = config.StartValue.ToString();
        }

        public void ChangeCount(int count)
        {
            _resElement.Description.text = count.ToString();
        }

    }
}
