pipeline {
  agent {
    docker {
      image 'buildtools2017'
    }

  }
  stages {
    stage('echo') {
      steps {
        bat 'echo "hello"'
      }
    }
  }
}