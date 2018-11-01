pipeline {
  agent {
    docker {
      image 'buildtools2017'
    }

  }
  stages {
    stage('checkout') {
      steps {
        checkout scm
      }
    }
  }
}