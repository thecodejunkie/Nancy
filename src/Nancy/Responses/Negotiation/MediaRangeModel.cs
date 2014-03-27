namespace Nancy.Responses.Negotiation
{
    using System;

    /// <summary>
    /// Stores model information for a <see cref="MediaRange"/> mapping when using
    /// content negotiation.
    /// </summary>
    public class MediaRangeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRangeModel"/> class.
        /// </summary>
        public MediaRangeModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRangeModel"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the that will be returned by <see cref="Factory"/>.</param>
        /// <param name="model">The model that will be returned by <see cref="Factory"/>.</param>
        public MediaRangeModel(Type type, dynamic model)
            : this(type, () => model)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRangeModel"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the that will be returned by <see cref="Factory"/>.</param>
        /// <param name="factory">The model factory that will be assigned to <see cref="Factory"/>.</param>
        public MediaRangeModel(Type type, Func<dynamic> factory)
        {
            this.Type = type;
            this.Factory = factory;
        }

        /// <summary>
        /// Gets or sets the model factory.
        /// </summary>
        /// <value>A <see cref="Func{T}"/> instance.</value>
        public Func<dynamic> Factory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the model.
        /// </summary>
        /// <value>A <see cref="Type"/> instance.</value>
        public Type Type { get; set; }
    }
}