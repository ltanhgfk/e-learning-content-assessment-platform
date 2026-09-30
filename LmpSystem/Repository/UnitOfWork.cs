using LmpSystem.Models;


namespace LmpSystem.Repository
{
    public  class UnitOfWork
    {
        private readonly LmpSystemEntities context = new LmpSystemEntities();

        private LmpInfoRepository _lmpInfoRepository;
        private CategoryRepository _categoryRepository;
        private UserRepository _userRepository;
        private UserScoreRepository _userScoreRepository;

        //private TeacherRepository _teacherRepository;
        //private TeacherReviewRepository _teacherReviewRepository;
        
        private TagRepository _tagRepository;
        private QuestionRepository _questionRepository;
        private LessonRepository _lessonRepository;

        //private ProductRepository _productRepository;
        //private ProductReviewRepository _productReviewRepository;
        //private ProductTagRepository _productTagRepository;
        //private PostImageRepository _productImageRepository;       
        
        private PostRepository _postRepository;
        private PostReviewRepository _postReviewRepository;
        //private PostTagRepository _postTagRepository;
        private PostImageRepository _postImageRepository;

        private OrderRepository _orderRepository;        
        private InvoiceRepository _invoiceRepository;

        private FeedbackRepository _feedbackRepository;
        
        /**************Bo*****************************/
        //private WebInfoRepository _infoRepository;
        //private PostRepository _postRepository;
        //private SeriesRepository _seriesRepository;        
        //private HotPostRepository _hotPostRepository;
        /*********************************************/
        public UnitOfWork(LmpSystemEntities _context)
        {
            this.context = _context;
        }
        public LmpSystemEntities Context
        {
            get
            {
                return context;
            }
        }

        public LmpInfoRepository LmpInfoRepository
        {
            get
            {
                if (_lmpInfoRepository == null)
                {
                    _lmpInfoRepository = new LmpInfoRepository(context);

                }
                return _lmpInfoRepository;
            }
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(context);

                }
                return _categoryRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(context);

                }
                return _userRepository;
            }
        }

        public UserScoreRepository UserScoreRepository
        {
            get
            {
                if (_userScoreRepository == null)
                {
                    _userScoreRepository = new UserScoreRepository(context);

                }
                return _userScoreRepository;
            }
        }


        //public TeacherRepository TeacherRepository
        //{
        //    get
        //    {
        //        if (_teacherRepository == null)
        //        {
        //            _teacherRepository = new TeacherRepository(context);

        //        }
        //        return _teacherRepository;
        //    }
        //}

        //public TeacherReviewRepository TeacherReviewRepository
        //{
        //    get
        //    {
        //        if (_teacherReviewRepository == null)
        //        {
        //            _teacherReviewRepository = new TeacherReviewRepository(context);

        //        }
        //        return _teacherReviewRepository;
        //    }
        //}

        public QuestionRepository QuestionRepository
        {
            get
            {
                if (_questionRepository == null)
                {
                    _questionRepository = new QuestionRepository(context);

                }
                return _questionRepository;
            }
        }

        public LessonRepository LessonRepository
        {
            get
            {
                if (_lessonRepository == null)
                {
                    _lessonRepository = new LessonRepository(context);

                }
                return _lessonRepository;
            }
        }

        //public ProductRepository ProductRepository
        //{
        //    get
        //    {
        //        if (_productRepository == null)
        //        {
        //            _productRepository = new ProductRepository(context);

        //        }
        //        return _productRepository;
        //    }
        //}

        public PostImageRepository PostImageRepository
        {
            get
            {
                if (_postImageRepository == null)
                {
                    _postImageRepository = new PostImageRepository(context);

                }
                return _postImageRepository;
            }
        }

        public PostReviewRepository PostReviewRepository
        {
            get
            {
                if (_postReviewRepository == null)
                {
                    _postReviewRepository = new PostReviewRepository(context);

                }
                return _postReviewRepository;
            }
        }

        //public ProductTagRepository ProductTagRepository
        //{
        //    get
        //    {
        //        if (_productTagRepository == null)
        //        {
        //            _productTagRepository = new ProductTagRepository(context);

        //        }
        //        return _productTagRepository;
        //    }
        //}


        public TagRepository TagRepository
        {
            get
            {
                if (_tagRepository == null)
                {
                    _tagRepository = new TagRepository(context);

                }
                return _tagRepository;
            }
        }

        public InvoiceRepository InvoiceRepository
        {
            get
            {
                if (_invoiceRepository == null)
                {
                    _invoiceRepository = new InvoiceRepository(context);

                }
                return _invoiceRepository;
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(context);

                }
                return _orderRepository;
            }
        }

        public PostRepository PostRepository
        {
            get
            {
                if(_postRepository== null)
                {
                    _postRepository = new PostRepository(context);
                    
                }
                return _postRepository;
            }
        }

        //public PostReviewRepository PostReviewRepository
        //{
        //    get
        //    {
        //        if (_postReviewRepository == null)
        //        {
        //            _postReviewRepository = new PostReviewRepository(context);

        //        }
        //        return _postReviewRepository;
        //    }
        //}

        //public PostTagRepository PostTagRepository
        //{
        //    get
        //    {
        //        if (_postTagRepository == null)
        //        {
        //            _postTagRepository = new PostTagRepository(context);

        //        }
        //        return _postTagRepository;
        //    }
        //}



        public FeedbackRepository FeedbackRepository
        {
            get
            {
                if (_feedbackRepository == null)
                {
                    _feedbackRepository = new FeedbackRepository(context);

                }
                return _feedbackRepository;
            }
        }



        public void Commit()
        {
            context.SaveChanges();
        }
    }
    
}